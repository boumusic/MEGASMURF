using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitEncyclopedia", menuName = "Gameplay/UnitEncylopedia", order = 150)]
public class UnitEncyclopedia : ScriptableObject
{
    public UnitBlueprint[] unitBlueprints;
}
