using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Base", menuName = "Gameplay/UnitBase", order = 150)]
public class UnitBase : ScriptableObject
{
    public BaseUnitType unitType;

    public AttackPattern[] attackPatterns;
    public MovementPattern[] movementPatterns;
    public UnitStatistics unitStats;
}
