using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitDeathSettings
{
    public float delayToggleVisuals = 0.2f;
    public float delayToggleGameObject = 0.5f;
}

[CreateAssetMenu(fileName = "New Unit General Settings", menuName = "Visuals/UnitGeneralSettings", order = 150)]
public class UnitGeneralSettings : ScriptableObject
{
    [Header("Movement")]
    [Range(0.01f, 0.3f)]
    public float moveSpeed = 0.07f;
    public float forwardSmooth = 0.07f;

    [Header("Death")]
    public UnitDeathSettings shapeDeath;
    public UnitDeathSettings enemyDeath;
    public UnitDeathSettings playerDeath;
}
