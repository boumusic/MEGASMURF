using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShapeUnit : Unit
{
    [Header("Components")]
    [SerializeField] private UnitMergeAnimator unitMergeAnimator;

    public BaseUnitType UnitType => BaseUnitType.ShapeComposite;
    public Equipement equipement { get; set; }

    private List<ShapeUnit> mergedUnits;
    public bool IsUnitComposite => mergedUnits.Count > 0;
    public int UnitMergeLevel => mergedUnits.Count;

    public ShapeUnit HeadUnit => (mergedUnits.Count > 1) ? mergedUnits[1] : null;
    public ShapeUnit ArmUnit => (mergedUnits.Count > 0) ? mergedUnits[0] : null;
    public ShapeUnit LegUnit => this;
    
    public override int MaxHealth
    {
        get
        {
            int maxHealth = unitBase.unitStats.maxHealth;
            foreach(ShapeUnit shape in mergedUnits)
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

        Stack<Tile> path = new Stack<Tile>();
        for (int i = 0; i < 20; i++)
        { 
          
            path.Push(Board.Instance.GetTile((int)UnityEngine.Random.Range(0, 10), (int)UnityEngine.Random.Range(0, 10)));
        }

        MoveTo(path);
    }

    /// <summary>
    /// Takes a unit and merges it on top of itself.
    /// </summary>
    /// <param name="shape">The shape that will be merged on top of this shape.</param>
    public void MergeWithAlly(ShapeUnit shape)
    {
        if (shape.UnitMergeLevel == 0)
        {
            if (UnitMergeLevel < 2)
            {
                mergedUnits.Add(shape);
                shape.unitMergeAnimator.MergeOnTopOf(this);
                // Autre check 
                // Vanish d'equipement + Refund
            }
            else
                Debug.LogError("Illicite Merge: bottom unit is already at max level");
        }
        else
            Debug.LogError("Illicite Merge: intiating unit is not level 0");
    }
}
