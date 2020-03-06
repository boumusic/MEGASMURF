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
            //combination[1] = (int)();
        }
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
