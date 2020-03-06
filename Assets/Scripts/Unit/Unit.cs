﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    Fresh,
    Moved,
    Used
}

public class Unit : LevelElement
{
    [Header("Components")]
    [SerializeField] private UnitAnimator unitAnimator;

    public Vector2 debugTile;

    public UnitBase unitBase;     //Passage en UnitBase

    protected Tile currentTile;
    public virtual Tile CurrentTile { get; protected set; }
    
    public virtual BaseUnitType UnitType => unitBase.unitType;

    public UnitState CurrentUnitState { get; private set; }

    public virtual int UnitMergeLevel => 0;

    //A Initialiser
    public float CurrentHitPoint { get; protected set; }

    public virtual int MaxHealth => unitBase.unitStats.maxHealth;
    public virtual int Damage => unitBase.unitStats.damage;

    public virtual AttackPattern UnitAttackPattern => unitBase.attackPatterns[0];
    public virtual MovementPattern UnitMovementPattern => unitBase.movementPatterns[0];
    public virtual UnitStatistics UnitStats => unitBase.unitStats;

    public UnitAnimator UnitAnimator { get => unitAnimator; }

    protected virtual void Awake()
    {
        currentTile = null;
    }

    public virtual void Start()
    {
        FaceCamera();
    }

    

    public virtual void SetUnitPosition(Tile tile)
    {
        CurrentTile = tile;
        transform.position = tile.transform.position;
    }

    public void RemoveFromBoard()
    {
        CurrentTile = null;
    }

    public void DebugSetUnitPosition()
    {
        SetUnitPosition(Board.Instance.GetTile(debugTile));
        DebugResetHealth();
    }

    public virtual void DebugResetHealth()
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
        BecomeMoved();
    }

    public virtual void MoveTo(Stack<Tile> path, System.Action action)
    {
        // Lancer une coroutine qui fait parcourir le chemin
        StartCoroutine(MovingTo(path, action));
        BecomeMoved();
    }

    private IEnumerator MovingTo(Stack<Tile> path, System.Action action)
    {
        SetAnimatorMoving(true);

        while (path.Count > 0)
        {
            if(path.Count == 1)
            {
                if(path.Peek().type == TileType.Ally)
                {
                    (path.Pop().unit as ShapeUnit).InitiateMergeAlly(this as ShapeUnit);
                    currentTile = null;
                    break;
                }
            }

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

        SetAnimatorMoving(false);
        FaceCamera();
        action?.Invoke();
    }

    public virtual void SetAnimatorMoving(bool moving)
    {
        unitAnimator.SetIsMoving(moving);
    }

    public void FaceCamera()
    {
        Vector3 forward = GameCamera.Instance.Forward;
        transform.forward = new Vector3(-forward.x, 0f, -forward.z);
    }

    public virtual void Attack(List<Tile> tiles)
    {
        switch (UnitAttackPattern.type)
        {
            case AttackPatternType.All:
                foreach (Tile tile in tiles)
                {
                    if ((tile.unit?.GetComponent<Unit>()) != null)
                        tile.unit.TakeDamage(this);
                }
                break;

            case AttackPatternType.Single:
                if ((tiles[0].unit?.GetComponent<Unit>()) != null)
                    tiles[0].unit.TakeDamage(this);
                break;

            case AttackPatternType.Slice:
                Stack<Tile> attackDestination = new Stack<Tile>();
                attackDestination.Push(tiles[tiles.Count - 1]);

                List<Tile> unitTiles = new List<Tile>();
                foreach (Tile tile in tiles)
                {
                    if ((tile.unit?.GetComponent<Unit>()) != null)
                        unitTiles.Add(tile);
                }

                StartCoroutine(MovingTo(attackDestination, null));
                
                foreach(Tile tile in unitTiles)
                {
                    tile.unit.TakeDamage(this);
                }
                break;
        }

        BecomeExhausted();
    }

    /// <summary>
    /// Method to receive damage
    /// </summary>
    /// <param name="unit">Unit who inflict the damage</param>
    public virtual void TakeDamage(Unit unit)
    {
        CurrentHitPoint -= unit.Damage;

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
