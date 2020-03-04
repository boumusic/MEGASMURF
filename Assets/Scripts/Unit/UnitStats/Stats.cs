using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitStats", menuName = "Gameplay/UnitStats", order = 150)]
public class Stats : ScriptableObject
{
    [Header("Base Stats")]
    public int maxHealth = 1;
    public Movement[] movements;
    public Attack[] attacks;
}
