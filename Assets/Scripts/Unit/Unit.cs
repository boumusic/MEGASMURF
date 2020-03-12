﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    Fresh,
    Moved,
    Used
}

public abstract class Unit : LevelElement
{
    [Header("Components")]
    [SerializeField] private UnitAnimator unitAnimator;
    [SerializeField] private Jauge hp;

    public Vector2 debugTile;

    public UnitBase unitBase;     //Passage en UnitBase
    public Sprite unitIcon;
    public Sprite selectedUnitIcon;
    public Sprite unitAction;
    

    protected Tile currentTile;
    public virtual Tile CurrentTile { get; protected set; }


    public virtual BaseUnitType UnitType => unitBase.unitType;

    public UnitState CurrentUnitState { get; private set; }

    public virtual int UnitMergeLevel => 0;

    public int SpawnID { get; set; }

    //A Initialiser
    private int currentHealth;
    public int CurrentHitPoint
    {
        get
        {
            return currentHealth;
        }
        protected set
        {
            currentHealth = value;
            UIManager.Instance.UpdateUnitHealth(this, value);
        }
    }

    public virtual int MaxHealth => unitBase.unitStats.maxHealth;
    public virtual int Damage => unitBase.unitStats.damage;

    public virtual AttackPattern UnitAttackPattern => unitBase.attackPatterns[0];
    public virtual MovementPattern UnitMovementPattern => unitBase.movementPatterns[0];
    public virtual UnitStatistics UnitStats => unitBase.unitStats;
    public Equipement CurrentEquipement { get; set; }

    public UnitAnimator UnitAnimator { get => unitAnimator; }

    private List<Tile> tempTileToAttack;
    private Action tempAction;

    protected virtual void Awake()
    {
        currentTile = null;
        tempTileToAttack = new List<Tile>();
        ResetHealth();
    }

    public virtual void Start()
    {
        FaceCamera();
        if (hp)
            hp.UpdateJauge(CurrentHitPoint, MaxHealth);
    }

    public virtual void SpawnUnit(Tile tile)
    {
        SetUnitPosition(tile);
    }

    public virtual void UnspawnUnit()
    {
        if (currentTile.unit != null)
        {
            currentTile.type = TileType.Free;
        }
        currentTile.unit = null;
        currentTile = null;
        BattleManager.Instance.RemoveUnitFromPlay(this);
        gameObject.SetActive(false);
    }

    public void RemoveFromBoard()
    {
        CurrentTile = null;
    }

    public virtual void SetUnitPosition(Tile tile)
    {
        CurrentTile = tile;
        transform.position = tile.transform.position;
    }

    public void DebugSetUnitPosition()
    {
        SetUnitPosition(Board.Instance.GetTile(debugTile));
        ResetHealth();
    }

    public virtual void ResetHealth()
    {
        CurrentHitPoint = MaxHealth;
    }

    public virtual void FreshenUp()
    {
        CurrentUnitState = UnitState.Fresh;
        //if stunned => Used State? ou state <-> Stunned + 1
        //if Used => Fresh
    }

    public virtual void MoveTo(Stack<Tile> path)
    {
        StartCoroutine(MovingTo(path, null));
    }

    public virtual void MoveTo(Stack<Tile> path, System.Action action)
    {
        // Lancer une coroutine qui fait parcourir le chemin
        StartCoroutine(MovingTo(path, action));
    }

    private IEnumerator MovingTo(Stack<Tile> path, System.Action action)
    {
        SetAnimatorMoving(true);

        TileType tempType = TileType.Free;

        if (currentTile != null)
        {
            currentTile.unit = null;
            tempType = currentTile.type;
            currentTile.type = TileType.Free;
        }
        while (path.Count > 0)
        {
            Tile destinationTile = path.Pop();
            Vector3 pos = destinationTile.transform.position;
            transform.forward = (pos - transform.position).normalized;
            while (transform.position != pos)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, UnitStats.moveSpeed);
                yield return new WaitForFixedUpdate();
            }
            CurrentTile = destinationTile;
        }

        if (currentTile != null) {
            currentTile.unit = this;
            currentTile.type = tempType;
        }

        SetAnimatorMoving(false);
        FaceCamera();
        
        BecomeMoved();

        action?.Invoke();
    }

    public virtual void SetAnimatorMoving(bool moving)
    {
        if (unitAnimator != null)
        {
            unitAnimator.SetIsMoving(moving);
        }
    }

    public void FaceCamera()
    {
        Vector3 forward = GameCamera.Instance.Forward;
        transform.forward = new Vector3(-forward.x, 0f, -forward.z);
    }

    public virtual void Action(List<Tile> tiles, Action action = null)
    {
        Attack(tiles, action);
    }

    public virtual void Attack(List<Tile> tiles, Action action = null)
    {
        switch (UnitAttackPattern.type)
        {
            case AttackPatternType.All:
                foreach (Tile tile in tiles)
                {
                    if (tile.unit != null)
                        tile.unit.TakeDamage(this);
                }
                BecomeExhausted();
                action?.Invoke();
                break;

            case AttackPatternType.Single:
                if (tiles.Count > 0 && tiles[0].unit != null)
                    tiles[0].unit.TakeDamage(this);
                BecomeExhausted();
                action?.Invoke();
                break;

            case AttackPatternType.Slice:                                                       //Bricolage a reprendre avec les anims (sequenceur)
                Stack<Tile> attackDestination = new Stack<Tile>();
                if(tiles.Count > 1) 
                {
                    attackDestination.Push(tiles[tiles.Count - 1]);
                }

                
                foreach (Tile tile in tiles)
                {
                    if (tile.unit != null)
                    {
                        tempTileToAttack.Add(tile);
                        tempAction = action;
                    }
                }

                MoveTo(attackDestination, OnAttackAnimationEnd);
                break;
        }
    }

    private void OnAttackAnimationEnd()
    {
        foreach (Tile tile in tempTileToAttack)
        {
            tile.unit.TakeDamage(this);
        }
        BecomeExhausted();
        tempAction?.Invoke();
    }

    /// <summary>
    /// Method to receive damage
    /// </summary>
    /// <param name="unit">Unit who inflict the damage</param>
    public virtual void TakeDamage(Unit unit)
    {
        Debug.Log(gameObject.name + " took " + unit.Damage + " damage from " + unit.gameObject.name);
        CurrentHitPoint -= unit.Damage;
        Debug.Log("He now has " + CurrentHitPoint);
        if(hp)hp.UpdateJauge(CurrentHitPoint, MaxHealth);

        if (CurrentHitPoint <= 0)
        {
            Die();
            unit.OnKillScored(this);
        }
    }

    /// <summary>
    /// Callback method to enable killer specific behaviour (like taking dead unit position)
    /// </summary>
    /// <param name="unit"> Killed unit </param>
    protected virtual void OnKillScored(Unit unit)
    {

    }

    /// <summary>
    /// Execute all the action needed upon unit death
    /// </summary>
    protected virtual void Die()
    {
        if(currentTile != null)
        {
            currentTile.type = TileType.Free;
        }
        BattleManager.Instance.RemoveUnitFromPlay(this);
        //animation
        gameObject.SetActive(false);
    }

    public virtual void BecomeMoved()
    {
        CurrentUnitState = UnitState.Moved;
    }

    public virtual void BecomeExhausted()
    {
        CurrentUnitState = UnitState.Used;
    }
}
