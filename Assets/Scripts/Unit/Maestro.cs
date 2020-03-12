using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Unit
{

    private int maxShapes;

    [HideInInspector]
    public int rangeLvl;
    public override MovementPattern UnitMovementPattern => HasInfiniteMoveRange ? unitBase.movementPatterns[unitBase.movementPatterns.Length-1] : unitBase.movementPatterns[rangeLvl];

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
                currentTile = null;
            }

            if (value != null)
            {
                currentTile = value;
                currentTile.unit = this;
                currentTile.type = TileType.Obstacle;               //LUL
                RecoltShapeMud();
                if (value is ExitTile)
                {
                    SpawnID = ((ExitTile)value).id;
                    //Board.Instance.NextRoom();
                }
                else
                {
                    SpawnID = -1;
                }
                if(value is Spawner)
                {
                    ((Spawner)value).activeSpawn = false;
                }
                else if(value is ImmediateSpawner)
                {
                    ((ImmediateSpawner)value).activeSpawn = false;
                }
            }
        }
    }

    public override void SpawnUnit(Tile tile)
    {
        base.SpawnUnit(tile);
        BattleManager.Instance.AddUnitToPlayerUnitList(0,gameObject);
    }

    public void InitMaestro()
    {
        maxShapes = 4 + GameManager.SkillTree.CheckEffect(SkillType.IncreaseArmy);
        rangeLvl = GameManager.SkillTree.CheckEffect(SkillType.MaestroMobility);
        unitBase.unitStats.maxHealth = 3 + GameManager.SkillTree.CheckEffect(SkillType.MaestroLife);
    }

    public override void Action(List<Tile> tile, Action action = null)
    {
        SummonUnit(BattleManager.Instance.SelectedUnitTypeToBeSummon, tile[0], action);
    }

    public override void Attack(List<Tile> tiles, Action action = null)
    {
        //No Attack
    }

    public void SummonUnit(BaseUnitType unitType, Tile tile, Action action = null)
    {
        GameObject newUnitGameObject = UnitFactory.Instance.CreateUnit(unitType);
        ShapeUnit newUnit;
        if ((newUnit = newUnitGameObject.GetComponent<ShapeUnit>()) != null)
        {
            BattleManager.Instance.AddUnitToPlayerUnitList(BattleManager.Instance.CurrentPlayerID, newUnitGameObject);
            newUnit.GetComponent<ShapeUnit>()?.SetUnitPosition(tile);
            //animation(action)
            (UnitAnimator as MaestroUnitAnimator).SpellAnim();
            action?.Invoke();
        }
        BecomeExhausted();
    }

    public void RecoltShapeMud()
    {
        if(currentTile.MudAmount > 0)
        {
            GameManager.ShapeMud += currentTile.MudAmount;
            currentTile.MudAmount = 0;
            //Animation
            //Remove Mud asset
        }
    }
}
