using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    IncreaseArmy = 0,
    MoreLoot = 4,
    MaestroMobility = 8,
    MaestroLife = 12,
    ReduceCosts = 16,
    SaferZone = 20,
}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Gameplay/Skill", order = 200)]
public class Skill : ScriptableObject
{

    public string skillName;
    public string description;

}
