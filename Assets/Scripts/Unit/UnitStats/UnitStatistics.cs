using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitStatistics", menuName = "Gameplay/UnitStatistics", order = 150)]
public class UnitStatistics : ScriptableObject
{
    public int maxHealth;
    public int damage;
}
