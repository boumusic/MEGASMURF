﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Brain UnitBrain { get; set; }

    public override Tile CurrentTile
    {
        get => currentTile;
        protected set
        {
            if (currentTile != null)
            {
                currentTile.unit = null;
                currentTile.type = TileType.Free;
                currentTile = null;
            }

            if (value != null)
            {
                currentTile = value;
                currentTile.unit = this;
                currentTile.type = TileType.Enemy;
            }
        }
    }

    public void Sequence()
    {
        if(CurrentUnitState == UnitState.Fresh)
        {
            CallAttack();
        }
        if(CurrentUnitState == UnitState.Fresh)
        {
            Stack<Tile> path = UnitBrain.FindClosestEnemyPath();
            if (path != null && path.Count > 0)
            {
                MoveTo(path, CallAttack);
            }
        }
    }

    public void CallAttack()
    {
        List<Tile> targets = UnitBrain.FindEnemiesInRange();
        if (targets.Count > 0)
        {
            Attack(targets);
        }
    }

    private void Awake()
    {
        //UnitBrain = new Brain(this);
    }

    public override Color ColorInEditor()
    {
        return base.ColorInEditor();
    }
}
