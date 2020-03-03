using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Stats", menuName = "Gameplay/UnitStats", order = 150)]
public class Stats : ScriptableObject
{
    [Header("Base Stats")]
    public float health = 1f;
    public Movement movement;
    public Attack attack;
}
