using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelElementChunkSettings
{
    public LevelElement levelElement;
    public Vector3 pos;
}

[CreateAssetMenu(fileName = "New Level Chunk.asset", menuName = "Level/Level Chunk", order = 40)]
public class LevelChunk : ScriptableObject
{
    public ChunkDifficulty difficulty;

    public List<LevelElementChunkSettings> elements = new List<LevelElementChunkSettings>();

    public int selectedBrush = 0;
}
