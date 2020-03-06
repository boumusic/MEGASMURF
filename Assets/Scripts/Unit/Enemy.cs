using System;
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
                CurrentTile = null;
            }

            if (value != null)
            {
                currentTile = value;
                currentTile.unit = this;
                currentTile.type = TileType.Enemy;
            }
        }
    }

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
