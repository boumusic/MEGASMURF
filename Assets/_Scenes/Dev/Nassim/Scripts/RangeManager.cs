using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{

    public static RangeManager Instance;

    public int debugRangeX, debugRangeY, debugRangeMax;
    public bool debugRange;

    private Dictionary<Tile, List<Tile>> rangePaths;
    private Dictionary<Tile, List<Tile>> attackRange;
    private Stack<Tile> currentPath;
    private Tile unitTile;

    // Start is called before the first frame update
    void Start()
    {
        if (RangeManager.Instance == null)
        {
            RangeManager.Instance = this;
            rangePaths = new Dictionary<Tile, List<Tile>>();
            attackRange = new Dictionary<Tile, List<Tile>>();
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
        List<Vector2> rangeTest = new List<Vector2>();
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
        Range range = new Range();
        range.coords = rangeTest;
        if (rangeTest.Count > 0)
        {
            GetTilesInMovementRange(Board.Instance.GetTile(rangeTest[0]), range);
            DisplayMovementTiles();
        }
    }

    public void GetTilesInAttackRange(Tile startTile, AttackPattern pattern)
    {
        unitTile = startTile;
        attackRange.Clear();
        ProcessAttackRange(pattern);
    }

    public void GetTilesInMovementRange(Tile startTile, Range range)
    {
        unitTile = startTile;
        rangePaths.Clear();
        currentPath.Clear();
        ProcessMovementRange(startTile, null, range, debugRangeMax);
    }

    private void ProcessAttackRange(AttackPattern pattern)
    {
        Range correctedRange = pattern.range;
        foreach(Vector2 v in pattern.range.coords)
        {
            Tile check = Board.Instance.GetTile(v + unitTile.Coords);
            if (check == null || check.type == TileType.None || check.type == TileType.Obstacle)
            {
                correctedRange.coords.Remove(v + unitTile.Coords);
            }
        }
        switch (pattern.type)
        {
            case AttackPatternType.All:
                // Just check if there is at least one target in range
                foreach (Vector2 v in correctedRange.coords)
                {
                    Tile allCheck = Board.Instance.GetTile(v + unitTile.Coords);
                    if (allCheck != null && allCheck.type == TileType.Enemy)
                    {
                        foreach (Vector2 v2 in correctedRange.coords)
                        {
                            attackRange.Add(Board.Instance.GetTile(v2), Board.Instance.GetTiles(correctedRange.coords));
                        }
                        return;
                    }
                }
                attackRange.Clear();
                break;
            case AttackPatternType.Single:
                // Check for tiles with targets on them
                foreach (Vector2 v in correctedRange.coords)
                {
                    Tile singleCheck = Board.Instance.GetTile(v + unitTile.Coords);
                    if (singleCheck != null && singleCheck.type == TileType.Enemy)
                    {
                        attackRange.Add(singleCheck, new List<Tile> { singleCheck });
                    }
                }
                break;
            case AttackPatternType.Slice:
                // Check for free tiles with targets and no obstacle between them and unit tile 
                foreach (Vector2 v in correctedRange.coords)
                {
                    Tile sliceCheck = Board.Instance.GetTile(v + unitTile.Coords);
                    if(sliceCheck.type == TileType.Free)
                    {
                        bool clear = false;
                        List<Tile> between = new List<Tile>();
                        foreach(Tile t in Board.Instance.GetTilesBetween(unitTile, sliceCheck, false))
                        {
                            if(t.type == TileType.Enemy)
                            {
                                between.Add(t);
                            }
                            else if(t.type == TileType.Ally || t.type == TileType.Obstacle)
                            {
                                clear = true;
                            }
                        }
                        if (clear)
                        {
                            between.Clear();
                        }
                        if (between.Count > 0)
                        {
                            attackRange.Add(sliceCheck, between);
                        }
                    }
                }
                break;
        }
    }
    
    private void ProcessMovementRange(Tile tile, List<Tile> previous, Range comparedRange, int remaining)
    {
        List<Tile> newPrevious = new List<Tile>();
        if (previous != null)
        {
            if (tile.Equals(unitTile))
            {
                return;
            }
            if (!comparedRange.coords.Contains(tile.Coords + unitTile.Coords) || (tile.type != TileType.Ally && tile.type != TileType.Free))
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
            ProcessMovementRange(nextTile, newPrevious, comparedRange, remaining--);
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

    public void DisplayMovementTiles()
    {
        foreach (Tile tile in rangePaths.Keys)
        {
            tile.TriggerAnimation(TileAnim.Movement);
        }
    }

    public void DisplayAttackTiles()
    {
        foreach (Tile tile in rangePaths.Keys)
        {
            tile.TriggerAnimation(TileAnim.Attack);
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

    public Stack<Tile> GetCurrentPath()
    {
        Stack<Tile> orderedPath = new Stack<Tile>();
        do
        {
            orderedPath.Push(currentPath.Pop());
        }
        while (currentPath.Count > 0);
        return orderedPath;
    }

}
