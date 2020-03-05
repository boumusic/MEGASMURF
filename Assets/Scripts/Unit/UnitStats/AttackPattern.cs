using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackPatternType
{
    Single,
    All,
    Slice
}

[CreateAssetMenu(fileName = "NewAttackPattern", menuName = "Gameplay/AttackPattern", order = 150)]
public class AttackPattern : ScriptableObject
{
    public AttackPatternType type;
    public Range range;
}
