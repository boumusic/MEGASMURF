using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public virtual Stats UnitStats { get; protected set; }
    public virtual Movement UnitMovement { get; protected set; }
    public virtual Attack UnitAttack { get; protected set; }

    public float CurrentHitPoint { get; protected set; }
    public StatModifier UnitStatModifier { get; protected set; }

    public abstract void MoveTo(Tile tile);

    public abstract void Attack(Tile tile);
}
