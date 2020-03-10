using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTemplate
{
    [SerializeField]
    public int[] combination;
    [SerializeField]
    public int[] position;
    [SerializeField]
    public int equipment;

    public UnitTemplate(ShapeUnit shape)
    {
        combination = new int[3];
        combination[0] = (int)(shape.unitBase.unitType);
        position = new int[2];

        if(shape.ArmUnit != null)
        {
            combination[1] = (int)(shape.ArmUnit.unitBase.unitType);
        }
        else
        {
            combination[1] = (int)(BaseUnitType.NONE);
        }
        if (shape.ArmUnit != null && shape.HeadUnit != null)
        {
            combination[2] = (int)(shape.HeadUnit.unitBase.unitType);
        }
        else
        {
            combination[2] = (int)(BaseUnitType.NONE);
        }

        position[0] = (int)shape.CurrentTile.Coords.x;
        position[1] = (int)shape.CurrentTile.Coords.y;

        equipment = shape.equipement.id;
    }

    public ShapeUnit GetUnit()
    {
        ShapeUnit unit = new ShapeUnit();
        
        ShapeUnit legUnit = UnitFactory.Instance.CreateUnit((BaseUnitType)combination[0]).GetComponent<ShapeUnit>();
        ShapeUnit armUnit = UnitFactory.Instance.CreateUnit((BaseUnitType)combination[1]).GetComponent<ShapeUnit>();
        ShapeUnit headUnit = UnitFactory.Instance.CreateUnit((BaseUnitType)combination[2]).GetComponent<ShapeUnit>();

        legUnit.NonGampelayMerge(armUnit);
        legUnit.NonGampelayMerge(headUnit);

        return legUnit;
    }
}

[System.Serializable]
public class Team
{
    [SerializeField]
    public List<UnitTemplate> units;

    public Team(List<ShapeUnit> shapes)
    {
        foreach(ShapeUnit shape in shapes)
        {
            units.Add(new UnitTemplate(shape));
        }
    }

    public List<ShapeUnit> GetUnits()
    {
        List<ShapeUnit> shapes = new List<ShapeUnit>();
        foreach(UnitTemplate unit in units)
        {
            shapes.Add(unit.GetUnit());
        }
        return shapes;
    }
}

[System.Serializable]
public class Save
{
    [SerializeField]
    public int shapemud;
    [SerializeField]
    public Dictionary<int, bool> skilltree;
    
    /*[SerializeField]
    public Team team;*/

    public Save()
    {
        shapemud = GameManager.ShapeMud;
        skilltree = GameManager.SkillTree.tree;
        //team = new Team(GameManager.units);
    }
}
