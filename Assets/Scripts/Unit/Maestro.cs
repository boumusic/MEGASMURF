using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Unit
{

    private int maxShapes;

    [HideInInspector]
    public int rangeLvl;

    public int summonCost;

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
                currentTile.type = TileType.Obstacle;               //LUL
            }
        }
    }

    public void InitMaestro()
    {
        maxShapes = 4 + GameManager.SkillTree.CheckEffect(SkillType.IncreaseArmy);
        rangeLvl = GameManager.SkillTree.CheckEffect(SkillType.MaestroMobility);
        unitBase.unitStats.maxHealth = 3 + GameManager.SkillTree.CheckEffect(SkillType.MaestroLife);
    }

}
