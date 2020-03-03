using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDifficulty
{
    public float scrollSpeed = 1f;
}

[CreateAssetMenu(fileName = "New Level.asset", menuName = "Level/Level", order = 40)]
public class Level : ScriptableObject
{   
    public List<Room> rooms = new List<Room>();
}
