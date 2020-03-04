using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : LevelElement
{
    public UnitBase unitBase;     //Passage en UnitBase
    public Tile CurrentTile { get; protected set; }

    //A Initialiser
    public float CurrentHitPoint { get; protected set; }

    public virtual int MaxHealth => unitBase.unitStats.maxHealth;
    public virtual int Damage => unitBase.unitStats.damage;
    
    public virtual AttackPattern UnitAttackPattern => unitBase.attackPatterns[0];
    public virtual MovementPattern UnitMovementPattern => unitBase.movementPatterns[0];
    public virtual UnitStatistics UnitStats => unitBase.unitStats;

    public virtual void SetUnitPosition(Tile tile)
    {
        //Set Unit.tile et Tile.unit
        //Animation d'apparition
    }

    public virtual void MoveTo(Stack<Tile> path)
    {
        // Lancer une coroutine qui fait parcourir le chemin
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
        
        if(CurrentHitPoint <= 0)
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
