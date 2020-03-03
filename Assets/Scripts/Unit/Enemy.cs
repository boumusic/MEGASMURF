using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public override Attack UnitAttack => enemy.UnitAttack;
    public override Movement UnitMovement => enemy.UnitMovement;
    public override Stats UnitStats => enemy.UnitStats;

    public Attack enemyAttack;
    public Movement enemyMovement;
    public Stats enemyStats;

    private BaseUnit enemy;

    private void Awake()
    {
        enemy.UnitAttack = enemyAttack;
        enemy.UnitMovement = enemyMovement;
        enemy.UnitStats = enemyStats;
    }

    public override void MoveTo(Tile tile)
    {
        throw new NotImplementedException();
    }

    public override void Attack(Tile tile)
    {
        throw new NotImplementedException();
    }
}
