using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Unit
{
    public override Tile CurrentTile
    {
        get => currentTile;
        protected set
        {
            if(currentTile != null)
            {
                currentTile.unit = null;
                currentTile.type = TileType.Free;
            }

            currentTile = value;

            currentTile.unit = this;
            currentTile.type = TileType.Obstacle;                                           //LUL
        }
    }

    public int maxShapes;
    public float mudLootMultiplier;

    public int maxMoveRange;

    public int maxHealth;

    public float shopReductions;

    public int safeZoneSize;

    public List<Skill> unlockedSkills;

}
