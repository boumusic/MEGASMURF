using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Color spawnCol = new Color(0.2f, 0.96f, 0.56f, 1f);
    public static Color killCol = new Color(0.96f, 0.2f, 0.3f, 1f);
    public static Color gridCol = new Color(0.4f, 0.45f, 0.7f, 1f);
    
    private static LevelManager instance;
    public static LevelManager Instance { get { if (!instance) instance = FindObjectOfType<LevelManager>(); return instance; } }

    private Room next;

    [Header("Settings")]
    public Level settings;
    private float currentDifficulty = 0f;
    private float currentDistanceChunkLastRow = 0f;
 
    /*
    public void RequestNewChunkSpawn()
    {
        
        if (settings)
        {
            List<Room> potentialChunks = GetAllChunksOfCurrentDifficulty();
            int max = potentialChunks.Count;
            int index = Random.Range(0, max);
            Room newChunk = potentialChunks[index];
            SpawnRoom(newChunk);
        }
        
    }

    private void SpawnRoom(Room chunk)
    {
        
        currentDistanceChunkLastRow = DistanceLastRowSpawnPos();
        for (int i = 0; i < chunk.elements.Count; i++)
        {
            SpawnElement(i, chunk);
        }
        
    }

    private void SpawnElement(int i, Room chunk)
    {
        
        LevelElementChunkSettings element = chunk.elements[i];
        if (element.levelElement)
        {
            LevelElement pooled = PoolManager.Instance.GetEntityOfType(element.levelElement.GetType()) as LevelElement;
            pooled.gameObject.SetActive(true);
            Vector3 normalizedPos = chunk.elements[i].pos;

            float maxY = settings.levelWidth;

            float xPosNewElement = Utility.Interpolate(settings.spawnPos, settings.spawnPos + settings.columnSpacing * (chunkColumns - 1), 0, chunkColumns - 1, normalizedPos.x);
            float yPosNewElement = Utility.Interpolate(maxY, -maxY, 0, chunkRows - 1, normalizedPos.y);

            Vector3 pos = new Vector3(xPosNewElement - 0.1f, yPosNewElement, 0);

            pooled.SpawnPosition = pos;
            pooled.transform.position = pos;
            pooled.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        
    }
    */
}
