using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitInMerge
{
    OnTop,
    OnBottom
}

[CreateAssetMenu(fileName = "New Unit Name Settings", menuName = "Visuals/UnitNameSettings", order = 150)]
public class UnitNameSettings : ScriptableObject
{
    public List<string> names = new List<string>();
    public List<string> prefixes = new List<string>();
    public string search = "";

    public UnitInMerge nameForPrefix;

    public UnitInMerge nameComingFirst;
}
