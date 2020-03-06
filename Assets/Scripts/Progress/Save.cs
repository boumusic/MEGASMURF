using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTemplate
{
    [SerializeField]
    public int[] combination;
    [SerializeField]
    public int equipment;

    public UnitTemplate(ShapeUnit shape)
    {
        combination = new int[3];
        combination[0] = (int)(shape.unitBase.unitType);
        if(shape.ArmUnit != null)
        {
            combination[1] = (int)(shape.ArmUnit.unitBase.unitType);
        }
        else
        {
            combination[1] = (int)(BaseUnitType.NONE);
        }
        if (shape.ArmUnit != null)
        {
            combination[2] = (int)(shape.HeadUnit.unitBase.unitType);
        }
        else
        {
            combination[2] = (int)(BaseUnitType.NONE);
        }
        equipment = shape.equipement.id;
    }

    public ShapeUnit GetUnit()
    {
        ShapeUnit unit = new ShapeUnit();
        switch (combination[0])
        {
            case (int)BaseUnitType.NONE:
                return null;
            case (int)BaseUnitType.Circle:
                unit.unitBase = SaveManager.Instance.unitFactory.circle.unitBase;
                break;
            case (int)BaseUnitType.Square:
                unit.unitBase = SaveManager.Instance.unitFactory.square.unitBase;
                break;
            case (int)BaseUnitType.Triangle:
                unit.unitBase = SaveManager.Instance.unitFactory.triangle.unitBase;
                break;
        }
        switch (combination[1])
        {
            case (int)BaseUnitType.Circle:
                unit.ArmUnit.unitBase = SaveManager.Instance.unitFactory.circle.unitBase;
                break;
            case (int)BaseUnitType.Square:
                unit.ArmUnit.unitBase = SaveManager.Instance.unitFactory.square.unitBase;
                break;
            case (int)BaseUnitType.Triangle:
                unit.ArmUnit.unitBase = SaveManager.Instance.unitFactory.triangle.unitBase;
                break;
        }
        switch (combination[2])
        {
            case (int)BaseUnitType.Circle:
                unit.HeadUnit.unitBase = SaveManager.Instance.unitFactory.circle.unitBase;
                break;
            case (int)BaseUnitType.Square:
                unit.HeadUnit.unitBase = SaveManager.Instance.unitFactory.square.unitBase;
                break;
            case (int)BaseUnitType.Triangle:
                unit.HeadUnit.unitBase = SaveManager.Instance.unitFactory.triangle.unitBase;
                break;
        }
        return unit;
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
    [SerializeField]
    public Team team;

    public Save()
    {
        shapemud = GameManager.ShapeMud;
        skilltree = GameManager.SkillTree.tree;
        team = new Team(GameManager.units);
    }
}
