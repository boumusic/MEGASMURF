using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Color spawnCol = new Color(0.2f, 0.96f, 0.56f, 1f);
    public static Color killCol = new Color(0.96f, 0.2f, 0.3f, 1f);
    public static Color gridCol = new Color(0.4f, 0.45f, 0.7f, 1f);

    public static int chunkRows = 10;
    public static int chunkColumns = 10;

    private static LevelManager instance;
    public static LevelManager Instance { get { if (!instance) instance = FindObjectOfType<LevelManager>(); return instance; } }

    private LevelChunk next;

    [Header("Settings")]
    public LevelSettings settings;
    private float currentDifficulty = 0f;
    private float currentDistanceChunkLastRow = 0f;

    private void Update()
    {
        if (settings)
        {
            Difficulty();
        }

        UpdateChunkProgress();
    }

    private void Difficulty()
    {
        currentDifficulty += Time.deltaTime * settings.difficultyIncreaseSpeed;
    }

    private void UpdateChunkProgress()
    {
        currentDistanceChunkLastRow -= Time.deltaTime * GetScrollingSpeed();
        if (currentDistanceChunkLastRow <= 0)
        {
            RequestNewChunkSpawn();
        }
    }

    public float GetScrollingSpeed()
    {
        if (settings)
        {
            float t = settings.scrollingCurve.Evaluate(currentDifficulty);
            float spd = Mathf.Lerp(settings.minScrollingSpeed, settings.maxScrollingSpeed, t);
            return spd;
        }

        else return 1f;
    }

    private float DistanceLastRowSpawnPos()
    {
        return (chunkColumns - 1) * settings.columnSpacing;
    }

    public void RequestNewChunkSpawn()
    {
        if (settings)
        {
            //Debug.Log("Request");
            List<LevelChunk> potentialChunks = GetAllChunksOfCurrentDifficulty();
            int max = potentialChunks.Count;
            int index = Random.Range(0, max);
            LevelChunk newChunk = potentialChunks[index];
            SpawnChunk(newChunk);
        }
    }

    private void SpawnChunk(LevelChunk chunk)
    {
        currentDistanceChunkLastRow = DistanceLastRowSpawnPos();
        for (int i = 0; i < chunk.elements.Count; i++)
        {
            SpawnElement(i, chunk);
        }
    }

    private void SpawnElement(int i, LevelChunk chunk)
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

    private List<LevelChunk> GetAllChunksOfCurrentDifficulty()
    {
        if (settings)
        {
            ChunkDifficulty curr = CurrentDifficulty();

            List<LevelChunk> chunks = new List<LevelChunk>();
            for (int i = 0; i < settings.levelChunks.Count; i++)
            {
                if (settings.levelChunks[i].difficulty == curr)
                {
                    chunks.Add(settings.levelChunks[i]);
                }
            }

            if (chunks.Count == 0)
            {
                int randomIndex = Random.Range(0, settings.levelChunks.Count);
                chunks.Add(settings.levelChunks[randomIndex]);
            }

            return chunks;
        }
        else
        {
            return null;
        }
    }

    private ChunkDifficulty CurrentDifficulty()
    {
        int length = System.Enum.GetNames(typeof(ChunkDifficulty)).Length;
        int index = (int)(Utility.Interpolate(0, length - 1, 0, 1, currentDifficulty));
        return (ChunkDifficulty)index;
    }
}
