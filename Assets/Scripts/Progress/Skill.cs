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

public class Skill : ScriptableObject
{
    public int id;

    public string skillName;
    public string description;

}
