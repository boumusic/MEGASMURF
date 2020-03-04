using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Name Settings", menuName = "Visuals/UnitNameSettings", order = 150)]
public class UnitNameSettings : ScriptableObject
{
    public List<string> names = new List<string>();
    public string search = "";
}
