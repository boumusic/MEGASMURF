using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Unit
{

    private int maxShapes;

    [HideInInspector]
    public Range maestroRange;

    public Range rangeLvl0;
    public Range rangeLvl1;
    public Range rangeLvl2;
    public Range rangeLvl3;

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
        switch (GameManager.SkillTree.CheckEffect(SkillType.MaestroMobility))
        {
            case 1:
                maestroRange = rangeLvl1;
                break;
            case 2:
                maestroRange = rangeLvl2;
                break;
            case 3:
                maestroRange = rangeLvl3;
                break;
            default:
                maestroRange = rangeLvl0;
                break;
        }
        unitBase.unitStats.maxHealth = 3 + GameManager.SkillTree.CheckEffect(SkillType.MaestroLife);
    }

}
