using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitStatistics", menuName = "Gameplay/UnitStatistics", order = 150)]
public class UnitStatistics : ScriptableObject
{
    public int maxHealth;
    public int damage;
    [Range(0.01f, 1.5f)] public float moveSpeed = 0.1f;

    public float height = 2f;
}
