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

    public override void Action(List<Tile> tile)
    {
        SummonUnit(BattleManager.Instance.SelectedUnitTypeToBeSummon, tile[0]);
    }

    public override void Attack(List<Tile> tiles)
    {
        //No Attack
    }

    public void SummonUnit(BaseUnitType unitType, Tile tile)
    {
        GameObject newUnitGameObject = UnitFactory.Instance.CreateUnit(unitType);
        ShapeUnit newUnit;
        if ((newUnit = newUnitGameObject.GetComponent<ShapeUnit>()) != null)
        {
            BattleManager.Instance.AddUnitToPlayerUnitList(BattleManager.Instance.CurrentPlayerID, newUnitGameObject);
            newUnit.GetComponent<ShapeUnit>()?.SetUnitPosition(tile);
        }
    }
}
