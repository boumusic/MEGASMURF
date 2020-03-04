using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : LevelElement
{
    public virtual Stats UnitStats { get; protected set; }
    public virtual Movement UnitMovement { get; protected set; }
    public virtual Attack UnitAttack { get; protected set; }

    public Tile currentTile { get; protected set; }

    public float CurrentHitPoint { get; protected set; }

    public virtual void SetUnitPosition(Tile tile)
    {
        //Set Unit.tile et Tile.unit
        //Animation d'apparition
    }

    public virtual void MoveTo(Tile tile)
    {
        // recup un path (list de tile)
        // Lancer une coroutine qui fait parcourir le chemin
    }

    public virtual void Attack(Tile tile)
    {
        //Recup le path jusqu'a la cible
        //Anim d'attaque
        tile.unit.TakeDamage(this);
    }

    public virtual void TakeDamage(Unit unit)
    {
        CurrentHitPoint -= unit.UnitAttack.damage;
        
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
        //Check si c'est un boss
        MoveTo(unit.currentTile);
        //Anim particuliere?
    }

    protected virtual void Die()
    {
        // Animation Mort
        // Drop loot?
        // Desactivation/Destroy
    }
}
