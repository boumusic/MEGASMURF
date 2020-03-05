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

    public UnitBase unitBase;     //Passage en UnitBase
    public Tile CurrentTile { get; protected set; }
    public BaseUnitType UnitType => unitBase.unitType;


    public UnitState CurrentUnitState { get; private set; }     //State Machine

    public virtual int UnitMergeLevel => 0;

    //A Initialiser
    public float CurrentHitPoint { get; protected set; }

    public virtual int MaxHealth => unitBase.unitStats.maxHealth;
    public virtual int Damage => unitBase.unitStats.damage;

    public virtual AttackPattern UnitAttackPattern => unitBase.attackPatterns[0];
    public virtual MovementPattern UnitMovementPattern => unitBase.movementPatterns[0];
    public virtual UnitStatistics UnitStats => unitBase.unitStats;

    public UnitAnimator UnitAnimator { get => unitAnimator; }

    public virtual void Start()
    {
        FaceCamera();
    }

    public virtual void SetUnitPosition(Tile tile)
    {
        //Set Unit.tile et Tile.unit
        //Animation d'apparition
    }

    public virtual void BecomeFresh()
    {
        //Change StateMachine To Fresh State
    }

    public virtual void MovementMode()
    {
        //Fait apparaitre les ranges de déplacement
        //Active l'enregistrement de path
        //Attend un input
        //Declenche le déplacement
        //Rend la main au turnManager
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
        unitAnimator.SetIsMoving(true);

        while (path.Count > 0)
        {
            Vector3 pos = path.Pop().transform.position;
            transform.forward = (pos - transform.position).normalized;
            while (transform.position != pos)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, UnitStats.moveSpeed);
                yield return new WaitForFixedUpdate();
            }
        }

        unitAnimator.SetIsMoving(false);
        FaceCamera();
        action?.Invoke();
    }

    public void FaceCamera()
    {
        Vector3 forward = GameCamera.Instance.Forward;
        transform.forward = new Vector3(-forward.x, 0f, -forward.z);
    }

    public virtual void AttackMode()
    {
        //Fait apparaitre les ranges de déplacement
        //Attent un input
        //Déclenche l'attaque
        //Rend la main au turnManager
    }

    public virtual void Attack(Tile tile)
    {
        //Recup le path jusqu'a la cible
        //Anim d'attaque
        tile.unit.TakeDamage(this);
    }

    /// <summary>
    /// Method to inflict damage
    /// </summary>
    /// <param name="unit"></param>
    public virtual void TakeDamage(Unit unit)
    {
        CurrentHitPoint -= unit.UnitStats.damage;

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
        // Animation Mort
        // Drop loot?
        // Desactivation/Destroy
    }
}
