using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Brain UnitBrain { get; set; }
    public override Attack UnitAttack => enemy.UnitStats.attacks[0];
    public override Movement UnitMovement => enemy.UnitStats.movements[0];
    public override Stats UnitStats => enemy.UnitStats;

    public Stats enemyStats;

    private BaseUnit enemy;

    private void Awake()
    {
        enemy.UnitStats = enemyStats;
        UnitBrain = new Brain(this);
    }

    public override Color ColorInEditor()
    {
        return Color.red;
    }
}
