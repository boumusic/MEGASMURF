using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : ScriptableObject
{
    public BaseUnitType unitType;
    public AttackPattern[] attackPatterns;
    public MovementPattern[] movementPatterns;
    public UnitStatistics unitStats;
}
