using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Unit
{
    public override Attack UnitAttack => maestro.UnitStats.attack;
    public override Movement UnitMovement => maestro.UnitStats.movement;
    public override Stats UnitStats => maestro.UnitStats;

    public Attack maestroAttacks;
    public Movement maestroMovement;
    public Stats maestroStats;

    private BaseUnit maestro;

    private void Awake()
    {
        maestro.UnitStats = maestroStats;
    }

    public override void Attack(Tile tile)
    {
        
    }

    protected override void OnKillScored(Unit unit)
    {

    }
}
