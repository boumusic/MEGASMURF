using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Range
{
    [Range(1, 7)] public int size = 2;
    public List<Vector2> coords = new List<Vector2>();
}
