using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelElementRoomSettings
{
    public LevelElement levelElement;
    public Vector3 pos;
}

[CreateAssetMenu(fileName = "New Room.asset", menuName = "Level/Room", order = 140)]
public class Room : ScriptableObject
{
    public List<LevelElementRoomSettings> elements = new List<LevelElementRoomSettings>();

    public int selectedBrush = 0;
}
