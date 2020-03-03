using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeUnit : Ally
{
    public bool IsUnitComposite => HeadUnit != null;
    public int UnitMergeLevel => unitStack.Count;

    public override Attack UnitAttack => (ArmUnit != null) ? ArmUnit.UnitAttack : LegUnit.UnitAttack;
    public override Movement UnitMovement => LegUnit.UnitMovement;
    public override Stats UnitStats => throw new NotImplementedException();                                                         // TODO

    public Attack baseUnitAttack;
    public Movement baseUnitMovement;
    public Stats baseUnitStats;

    public BaseUnit HeadUnit => (unitStack.Count > 2) ? unitStack[2] : null;
    public BaseUnit ArmUnit => (unitStack.Count > 1) ? unitStack[1] : null;
    public BaseUnit LegUnit => (unitStack.Count > 0) ? unitStack[0] : null;

    private List<BaseUnit> unitStack;

    private void Awake()
    {
        LegUnit.UnitAttack = baseUnitAttack;
        LegUnit.UnitMovement = baseUnitMovement;
        LegUnit.UnitStats = baseUnitStats;

        unitStack = new List<BaseUnit>();
    }

    public override void MoveTo(Tile tile)
    {
        throw new NotImplementedException();
    }

    public override void Attack(Tile tile)
    {
        throw new NotImplementedException();
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
