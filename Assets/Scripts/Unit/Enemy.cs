using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Brain UnitBrain { get; set; }
    public override Attack UnitAttack => enemy.UnitStats.attack;
    public override Movement UnitMovement => enemy.UnitStats.movement;
    public override Stats UnitStats => enemy.UnitStats;

    public Stats enemyStats;

    private BaseUnit enemy;

    private void Awake()
    {
        enemy.UnitStats = enemyStats;
        UnitBrain = new Brain(this);
    }
}
