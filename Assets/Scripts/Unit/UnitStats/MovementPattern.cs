using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementPatternType
{
    Walk,
    Teleport
}

[CreateAssetMenu(fileName = "NewMovementPattern", menuName = "Gameplay/MovementPattern", order = 150)]
public class MovementPattern : ScriptableObject
{
    public MovementPatternType type;
    public Range range;
}
