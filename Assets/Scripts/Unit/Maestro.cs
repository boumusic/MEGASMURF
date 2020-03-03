using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Ally
{
    public override Attack UnitAttack => maestro.UnitAttack;
    public override Movement UnitMovement => maestro.UnitMovement;
    public override Stats UnitStats => maestro.UnitStats;

    public Attack maestroAttacks;
    public Movement maestroMovement;
    public Stats maestroStats;

    private BaseUnit maestro;

    private void Awake()
    {
        maestro.UnitAttack = maestroAttacks;
        maestro.UnitMovement = maestroMovement;
        maestro.UnitStats = maestroStats;
    }

    public override void Attack(Tile tile)
    {
        throw new System.NotImplementedException();
    }

    public override void MoveTo(Tile tile)
    {
        throw new System.NotImplementedException();
    }
}
