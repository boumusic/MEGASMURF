using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{

    public static RangeManager Instance;

    public int debugRangeX, debugRangeY;
    public bool debugRange;

    private Dictionary<Tile, List<Vector2Int>> rangePaths;
    private Tile unitTile;

    // Start is called before the first frame update
    void Start()
    {
        if (RangeManager.Instance == null)
        {
            RangeManager.Instance = this;
            rangePaths = new Dictionary<Tile, List<Vector2Int>>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debugRange)
        {
            debugRange = false;
            DebugRange();
        }
    }

    public void DebugRange()
    {
        List<Vector2Int> rangeTest = new List<Vector2Int>();
        for (int i = 0; i < debugRangeX; i++)
        {
            for (int j = 0; j < debugRangeY; j++)
            {
                if (Board.Instance.GetTile(i, j) != null)
                {
                    rangeTest.Add(Board.Instance.GetTile(i, j).Coords);
                }
            }
        }
        if (rangeTest.Count > 0)
        {
            GetTilesInRange(Board.Instance.GetTile(rangeTest[0]), rangeTest);
        }
    }

    public void GetTilesInRange(Tile startTile, List<Vector2Int> range)
    {
        unitTile = startTile;
        rangePaths.Clear();
        ProcessRange(startTile, null, range);
    }

    private void ProcessRange(Tile tile, List<Vector2Int> previous, List<Vector2Int> comparedRange)
    {
        List<Vector2Int> newPrevious = new List<Vector2Int>();
        if (previous != null)
        {
            if (tile.Equals(unitTile))
            {
                return;
            }
            if (!comparedRange.Contains(tile.Coords) || (tile.type != TileType.Ally && tile.type != TileType.Free))
            {
                return;
            }
            if (rangePaths.ContainsKey(tile))
            {
                if (rangePaths[tile].Count > previous.Count)
                {
                    rangePaths[tile] = previous;
                }
                else
                {
                    return;
                }
            }
            else
            {
                rangePaths.Add(tile, previous);
            }
            newPrevious.AddRange(previous);
            newPrevious.Add(tile.Coords);         
        }
        foreach (Tile nextTile in tile.GetNeighbors())
        {
            ProcessRange(nextTile, newPrevious, comparedRange);
        }
    }

    public void DisplayTiles()
    {
        foreach (Tile tile in rangePaths.Keys)
        {
            tile.TriggerAnimation(TileAnim.Movement);
        }
    }

}
