using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Name Settings", menuName = "Visuals/UnitNameSettings", order = 150)]
public class UnitGeneralSettings : ScriptableObject
{
    [Range(0.01f, 0.3f)]
    public float moveSpeed = 0.07f;
}
