using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShapeUnit : Unit
{
    [Header("Components")]
    [SerializeField] private UnitMergeAnimator unitMergeAnimator;
    [SerializeField] private Transform mergeParent;

    public BaseUnitType UnitType => BaseUnitType.ShapeComposite;
    public Equipement equipement { get; set; }

    private List<ShapeUnit> mergedUnits;
    public bool IsUnitComposite => mergedUnits.Count > 0;
    public override int UnitMergeLevel => mergedUnits.Count;

    public ShapeUnit HeadUnit => (mergedUnits.Count > 1) ? mergedUnits[1] : null;
    public ShapeUnit ArmUnit => (mergedUnits.Count > 0) ? mergedUnits[0] : null;
    public ShapeUnit LegUnit => this;

    public float Height => UnitMergeLevel * unitBase.unitStats.height;

    private string unitName = "";

    public override int MaxHealth
    {
        get
        {
            int maxHealth = unitBase.unitStats.maxHealth;
            foreach (ShapeUnit shape in mergedUnits)
            {
                maxHealth += shape.MaxHealth;
            }
            return maxHealth;
        }
    }
    public int Damage => (ArmUnit != null) ? ArmUnit.Damage : unitBase.unitStats.damage;

    public override AttackPattern UnitAttackPattern => (ArmUnit != null) ? ArmUnit.unitBase.attackPatterns[1] : unitBase.attackPatterns[0]; // Ajout range level 3 (item)
    public override MovementPattern UnitMovementPattern => unitBase.movementPatterns[(mergedUnits.Count > 0) ? 1 : 0];  // Ajout range level 3 (item)

    private void Awake()
    {
        mergedUnits = new List<ShapeUnit>();
    }

    private ShapeUnit shapeBeingMerged;

    public void InitiateMergeAlly(ShapeUnit shape)
    {
        InitiateMergeAlly(shape, null);
    }

    /// <summary>
    /// Takes a unit and merges it on top of itself.
    /// </summary>
    /// <param name="shape">The shape that will be merged on top of this shape.</param>
    public void InitiateMergeAlly(ShapeUnit shape, System.Action onFinished)
    {
        if (shape.UnitMergeLevel == 0)
        {
            if (UnitMergeLevel < 2)
            {
                onFinished += FinishedMerging;
                shapeBeingMerged = shape;
                mergedUnits.Add(shape);
                shape.unitMergeAnimator.MergeOnTopOf(this, onFinished);
                // Autre check 
                // Vanish d'equipement + Refund
            }
            else
                Debug.LogError("Illicite Merge: bottom unit is already at max level");
        }
        else
            Debug.LogError("Illicite Merge: intiating unit is not level 0");
    }

    private void FinishedMerging()
    {
        if (shapeBeingMerged)
        {
            shapeBeingMerged.transform.parent = mergeParent;
            shapeBeingMerged = null;
        }
    }
}
