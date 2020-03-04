using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{

    public static RangeManager Instance;

    public int debugRangeX, debugRangeY;
    public bool debugRange;

    private Dictionary<Tile, List<Tile>> path;
    private List<Tile> displayedRange;
    private Tile unitTile;

    // Start is called before the first frame update
    void Start()
    {
        if (RangeManager.Instance == null)
        {
            RangeManager.Instance = this;
            path = new Dictionary<Tile, List<Tile>>();
            displayedRange = new List<Tile>();
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
        List<Tile> rangeTest = new List<Tile>();
        for (int i = 0; i < debugRangeX; i++)
        {
            for (int j = 0; j < debugRangeY; j++)
            {
                if (Board.Instance.GetTile(i, j) != null)
                {
                    rangeTest.Add(Board.Instance.GetTile(i, j));
                }
            }
        }
        GetTilesInRange(rangeTest[0], rangeTest);
    }

    public void GetTilesInRange(Tile startTile, List<Tile> range)
    {
        unitTile = startTile;
        displayedRange.Clear();
        path.Clear();
        ProcessMovementRange(startTile, null, range, 20);
    }

    private void ProcessMovementRange(Tile tile, List<Tile> previous, List<Tile> comparedRange, int remain)
    {
        if (remain <= 0 && (tile.type != TileType.Ally && tile.type != TileType.Free) || (tile.isProcessed && previous != null && previous.Count > path[tile].Count) || !comparedRange.Contains(tile))
        {
            return;
        }
        else
        {
            if (!tile.Equals(unitTile) && !tile.isProcessed)
            {
                displayedRange.Add(tile);
                tile.isProcessed = true;
            }
            if (previous != null)
            {
                if (path.ContainsKey(tile))
                {
                    if (path[tile].Count > previous.Count)
                    {
                        path[tile] = previous;
                    }
                }
                else if (!tile.Equals(unitTile) && (tile.type == TileType.Ally || tile.type == TileType.Free))
                {
                    path.Add(tile, previous);
                }
            }
            List<Tile> newPrevious = new List<Tile>();
            if (previous != null && !tile.Equals(unitTile))
            {
                newPrevious.AddRange(previous);
                newPrevious.Add(tile);
            }
            foreach (Tile nextTile in tile.Neighbors)
            {
                ProcessMovementRange(nextTile, newPrevious, comparedRange, remain--);
            }
        }
    }

    public void DisplayTiles()
    {

    }
}
