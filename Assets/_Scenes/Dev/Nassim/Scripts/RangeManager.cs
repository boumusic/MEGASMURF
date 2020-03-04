using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{

    public static RangeManager Instance;

    public int debugRangeX, debugRangeY, debugRangeMax;
    public bool debugRange;

    private Dictionary<Tile, List<Tile>> rangePaths;
    private Stack<Tile> currentPath;
    private Tile unitTile;

    // Start is called before the first frame update
    void Start()
    {
        if (RangeManager.Instance == null)
        {
            RangeManager.Instance = this;
            rangePaths = new Dictionary<Tile, List<Tile>>();
            currentPath = new Stack<Tile>();
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
            DisplayTiles();
        }
    }

    public void GetTilesInRange(Tile startTile, List<Vector2Int> range)
    {
        unitTile = startTile;
        rangePaths.Clear();
        currentPath.Clear();
        ProcessRange(startTile, null, range, debugRangeMax);
    }

    private void ProcessRange(Tile tile, List<Tile> previous, List<Vector2Int> comparedRange, int remaining)
    {
        List<Tile> newPrevious = new List<Tile>();
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
            newPrevious.Add(tile);         
        }
        if(remaining <= 0)
        {
            return;
        }
        foreach (Tile nextTile in tile.GetNeighbors())
        {
            ProcessRange(nextTile, newPrevious, comparedRange, remaining--);
        }
    }

    public bool IsInRange(Tile tile)
    {
        if(rangePaths.ContainsKey(tile)) 
        {
            return true;
        }
        return false;
    }

    public void AddToCurrentPath(Tile tile)
    {
        if((tile.type != TileType.Ally && tile.type != TileType.Free) || IsInRange(tile))
        {
            if(currentPath.Count == 0)
            {
                SetShorterCurrentPath(tile);
            } 
            else 
            {
                if(tile.Equals(currentPath.Peek())) 
                {
                    return;
                }
                if(tile.IsNeighbor(currentPath.Peek()))
                {
                    if(rangePaths[tile].Count < currentPath.Count) 
                    {
                        if(currentPath.Contains(tile)) 
                        {
                            bool found;
                            do 
                            {
                                Tile peek = currentPath.Peek();
                                found = tile.Equals(peek) || currentPath.Count == 0;
                                if(!found)
                                {
                                    currentPath.Pop().TriggerAnimation(TileAnim.Movement);
                                }
                            }
                            while(!found);
                        } 
                        else 
                        {
                            SetShorterCurrentPath(tile);
                        }
                    } 
                    else
                    {
                        currentPath.Push(tile);
                    }
                } 
                else 
                {
                    SetShorterCurrentPath(tile);
                }
            }
            DisplayCurrentPath();
        } 
        else 
        {
            ClearCurrentPath();
        }
    }

    public void SetShorterCurrentPath(Tile tile)
    {
        foreach(Tile pathTile in currentPath) 
        {
            if(!rangePaths[tile].Contains(pathTile)) 
            {
                pathTile.TriggerAnimation(TileAnim.Movement);
            }
        }
        currentPath.Clear();
        foreach(Tile pathTile in rangePaths[tile]) 
        {
            currentPath.Push(pathTile);
        }
        currentPath.Push(tile);
    }

    public void DisplayTiles()
    {
        foreach (Tile tile in rangePaths.Keys)
        {
            tile.TriggerAnimation(TileAnim.Movement);
        }
    }

    public void DisplayCurrentPath()
    {
        foreach(Tile tile in currentPath)
        {
            tile.TriggerAnimation(TileAnim.MovementMouseOver);
        }
    }

    public void ClearTiles()
    {
        foreach(Tile tile in rangePaths.Keys)
        {
            tile.TriggerAnimation(TileAnim.None);
        }
        rangePaths.Clear();
        currentPath.Clear();
    }

    public void ClearCurrentPath()
    {
        foreach(Tile tile in currentPath) 
        {
            tile.TriggerAnimation(TileAnim.Movement);
        }
        currentPath.Clear();
    }

}
