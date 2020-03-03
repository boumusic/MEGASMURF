using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeUnit : Unit
{
    public BaseUnit baseUnit;

    public bool IsUnitComposite => mergedUnits.Count > 0;
    public int UnitMergeLevel => mergedUnits.Count;

    ///////////////////////////// Part To modify (UnitStats)
    public override Attack UnitAttack => (ArmUnit != null) ? ArmUnit.UnitStats.attacks[1] : baseUnitStats.attacks[0];
    public override Movement UnitMovement => baseUnitStats.movements[(mergedUnits.Count > 0) ? 1 : 0];
    public override Stats UnitStats => (mergedUnits.Count > 0) ?  compositeStats : baseUnitStats;
    /////////////////////////////

    public Stats baseUnitStats;                                                                                                                 //To Be Modified
    private Stats compositeStats;                                                                                                               //To Be Modified

    public BaseUnit HeadUnit => (mergedUnits.Count > 1) ? mergedUnits[1] : null;
    public BaseUnit ArmUnit => (mergedUnits.Count > 0) ? mergedUnits[0] : null;
    public BaseUnit LegUnit => baseUnit;

    public Equipement equipement { get; set; }

    private List<BaseUnit> mergedUnits;

    private void Awake()
    {
        mergedUnits = new List<BaseUnit>();
    }

    public void MergeWithAlly(ShapeUnit shape)
    {
        if (shape.UnitMergeLevel == 0)
        {
            if (UnitMergeLevel < 2)
                mergedUnits.Add(shape.baseUnit);
                // Autre check 
            else
                Debug.LogError("Illicite Merge: bottom unit is already at max level");
        }
        else
            Debug.LogError("Illicite Merge: intiating unit is not level 0");
    }
}
