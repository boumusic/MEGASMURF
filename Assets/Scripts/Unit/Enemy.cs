using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Brain UnitBrain { get; set; }

    private void Awake()
    {
        //UnitBrain = new Brain(this);
    }

    public override void SetUnitPosition(Tile tile)
    {
        CurrentTile = tile;
        transform.position = tile.transform.position;
        tile.unit = this;
        tile.type = TileType.Enemy;
    }

    public override Color ColorInEditor()
    {
        return Color.red;
    }
}
