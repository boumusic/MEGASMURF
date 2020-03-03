using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeUnit : Unit
{
    public bool IsUnitComposite => HeadUnit != null;
    public int UnitMergeLevel => unitStack.Count;

    public override Attack UnitAttack => (ArmUnit != null) ? ArmUnit.UnitStats.attack : LegUnit.UnitStats.attack;
    public override Movement UnitMovement => LegUnit.UnitStats.movement;
    public override Stats UnitStats => throw new NotImplementedException();                                                         // TODO

    public Stats baseUnitStats;

    public BaseUnit HeadUnit => (unitStack.Count > 2) ? unitStack[2] : null;
    public BaseUnit ArmUnit => (unitStack.Count > 1) ? unitStack[1] : null;
    public BaseUnit LegUnit => (unitStack.Count > 0) ? unitStack[0] : null;

    private List<BaseUnit> unitStack;

    private void Awake()
    {
        LegUnit.UnitStats = baseUnitStats;

        unitStack = new List<BaseUnit>();
    }

    public void MergeWithAlly(ShapeUnit shape)
    {
        if (shape.UnitMergeLevel == 1)
        {
            if (UnitMergeLevel < 3)
                unitStack.Add(shape.LegUnit);
            else
                Debug.LogError("Illicite Merge: bottom unit is already at max level");
        }
        else
            Debug.LogError("Illicite Merge: intiating unit is not level 1");
    }
}
