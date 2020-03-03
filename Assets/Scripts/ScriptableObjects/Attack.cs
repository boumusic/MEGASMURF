using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Gameplay/Attack", order = 150)]
public class Attack : ScriptableObject
{
    public float damage = 1f;
    public Range range;
}
