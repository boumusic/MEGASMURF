using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChunkDifficulty
{
    VeryEasy,
    Easy,
    Medium,
    Hard,
    VeryHard   
}

public class LevelDifficulty
{
    public float scrollSpeed = 1f;
}

[CreateAssetMenu(fileName = "New Level Settings.asset", menuName = "Level/Level Settings", order = 40)]
public class LevelSettings : ScriptableObject
{
    [Range(0.01f, 10)] public float columnSpacing = 1f;
    [Range(0, 10)] public float levelWidth = 5f;
    
    [Range(0, 30)] public float spawnPos = 10f;
    [Range(0, -30)] public float killPos = -10f;
    [Range(0, 0.5f)] public float delayBetweenEachElementSpawn = 0.05f;
    [Space]
    [Range(0f, 10f)] public float minScrollingSpeed = 1f;
    [Range(0f, 10f)] public float maxScrollingSpeed = 2f;
    public AnimationCurve scrollingCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0) });
    [Range(0.001f, 0.1f)] public float difficultyIncreaseSpeed = 0.001f;
    
    public List<LevelChunk> levelChunks = new List<LevelChunk>();


}
