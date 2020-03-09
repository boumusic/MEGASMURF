using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{

    public static RangeManager Instance;

    private Dictionary<Tile, List<Tile>> rangePaths;
    private Dictionary<Tile, List<Tile>> attackPaths;
    private Range fullRange;
    private List<Tile> attackRange;
    private Stack<Tile> currentPath;
    private Tile target;
    private Tile unitTile;
    private int maxPathfindingDepth;

    // Start is called before the first frame update
    void Start()
    {
        if (RangeManager.Instance == null)
        {
            RangeManager.Instance = this;
            maxPathfindingDepth = 100;
            rangePaths = new Dictionary<Tile, List<Tile>>();
            attackPaths = new Dictionary<Tile, List<Tile>>();
            attackRange = new List<Tile>();
            currentPath = new Stack<Tile>();
            fullRange = new Range();
            fullRange.coords = new List<Vector2>();
            for (int i = 0; i < Board.Instance.Columns; i++)
            {
                for (int j = 0; j < Board.Instance.Rows; j++)
                {
                    fullRange.coords.Add(new Vector2(i, j));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Tile> GetTilesInAttackRange(Tile startTile)
    {
        List<Tile> range = new List<Tile>();
        if (startTile == null)
        {
            return range;
        }
        unitTile = startTile;
        attackRange.Clear();
        ProcessAttackRange(startTile.unit.UnitAttackPattern);
        foreach (Tile tile in attackPaths.Keys)
        {
            range.Add(tile);
        }
        return range;

    }

    public List<Tile> GetTilesInMovementRange(Tile startTile)
    {
        unitTile = startTile;
        rangePaths.Clear();
        currentPath.Clear();
        switch (startTile.unit.UnitMovementPattern.type)
        {
            case MovementPatternType.Teleport:
                foreach (Vector2 v in startTile.unit.UnitMovementPattern.range.coords)
                {
                    Tile check = Board.Instance.GetTile(v + unitTile.Coords);
                    if (check != null && !AbortTileCondition(check, startTile.unit.UnitMovementPattern.range))
                    {
                        rangePaths.Add(check, new List<Tile>());
                    }
                }
                break;
            case MovementPatternType.Walk:
                ProcessMovementRange(startTile, null, startTile.unit.UnitMovementPattern.range, maxPathfindingDepth);
                break;
        }
        List<Tile> range = new List<Tile>();
        foreach (Tile tile in rangePaths.Keys)
        {
            range.Add(tile);
        }
        return range;
    }

    private bool CanTarget(Tile tile)
    {
        return tile != null && (((unitTile.type == TileType.Ally || unitTile.type == TileType.Obstacle) && tile.type == TileType.Enemy) || ((unitTile.type == TileType.Enemy && (tile.type == TileType.Obstacle || tile.type == TileType.Enemy))));
    }

    private void ProcessAttackRange(AttackPattern pattern)
    {
        Range correctedRange = new Range();
        correctedRange.coords = new List<Vector2>();
        correctedRange.coords.AddRange(pattern.range.coords);
        for (int i = 0; i < correctedRange.coords.Count; i++)
        {
            correctedRange.coords[i] += unitTile.Coords;
        }
        foreach (Vector2 v in pattern.range.coords)
        {
            Tile check = Board.Instance.GetTile(v + unitTile.Coords);
            if (check == null || check.type == TileType.None)
            {
                correctedRange.coords.Remove(v + unitTile.Coords);
            }
        }
        attackRange = Board.Instance.GetTiles(correctedRange.coords);
        switch (pattern.type)
        {
            case AttackPatternType.All:
                // Just check if there is at least one target in range
                foreach (Vector2 v in correctedRange.coords)
                {
                    Tile allCheck = Board.Instance.GetTile(v);
                    if (CanTarget(allCheck))
                    {
                        foreach (Vector2 v2 in correctedRange.coords)
                        {
                            allCheck = Board.Instance.GetTile(v2);
                            if (allCheck != null)
                            {
                                if (attackPaths.ContainsKey(allCheck))
                                {
                                    attackPaths[allCheck] = Board.Instance.GetTiles(correctedRange.coords);
                                }
                                else
                                {
                                    attackPaths.Add(allCheck, Board.Instance.GetTiles(correctedRange.coords));
                                }
                            }
                        }
                        return;
                    }
                }
                break;
            case AttackPatternType.Single:
                // Check for tiles with targets on them
                foreach (Vector2 v in correctedRange.coords)
                {
                    Tile singleCheck = Board.Instance.GetTile(v);
                    if (CanTarget(singleCheck))
                    {
                        attackPaths.Add(singleCheck, new List<Tile> { singleCheck });
                    }
                }
                break;
            case AttackPatternType.Slice:
                // Check for free tiles with targets and no obstacle between them and unit tile 
                foreach (Vector2 v in correctedRange.coords)
                {
                    Tile sliceCheck = Board.Instance.GetTile(v);
                    if (sliceCheck != null && sliceCheck.type == TileType.Free)
                    {
                        bool clear = false;
                        List<Tile> between = new List<Tile>();
                        foreach (Tile t in Board.Instance.GetTilesBetween(unitTile, sliceCheck, false))
                        {
                            if (CanTarget(t))
                            {
                                between.Add(t);
                            }
                            else if (t.type != TileType.Free)
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
                            if (attackPaths.ContainsKey(sliceCheck))
                            {
                                attackPaths[sliceCheck] = between;
                            }
                            else
                            {
                                attackPaths.Add(sliceCheck, between);
                            }
                        }
                    }
                }
                break;
        }
    }

    private bool AbortTileCondition(Tile tile, Range comparedRange)
    {
        if (tile == null)
        {
            return true;
        }
        // Tile is unit tile or tile is occupied by full totem unit
        if (tile.Equals(unitTile) || (tile.unit != null && tile.unit.UnitMergeLevel > 2))
        {
            return true;
        }
        // Unit is totem or enemy or maestro and tile is occupied by ally
        if ((unitTile.unit.UnitMergeLevel > 0 || unitTile.type == TileType.Enemy || unitTile.type == TileType.Obstacle) && tile.type == TileType.Ally)
        {
            return true;
        }
        // Tile is out of range or tile is not free or occupied by ally
        if (!comparedRange.coords.Contains(tile.Coords - unitTile.Coords) || (tile.type != TileType.Free && tile.type != TileType.Ally))
        {
            return true;
        }
        return false;
    }

    private void ProcessMovementRange(Tile tile, List<Tile> previous, Range comparedRange, int endlessLoopSecurity)
    {
        List<Tile> newPrevious = new List<Tile>();
        if (tile == null || endlessLoopSecurity <= 0)
        {
            return;
        }
        if (previous != null)
        {
            if (AbortTileCondition(tile, comparedRange))
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
            if (tile.type == TileType.Ally)
            {
                return;
            }
        }
        foreach (Tile nextTile in tile.GetNeighbors())
        {
            ProcessMovementRange(nextTile, newPrevious, comparedRange, endlessLoopSecurity--);
        }
    }

    public Queue<Tile> AIPathfinding(Tile aiTile)
    {
        unitTile = aiTile;
        FindWayToUnits(aiTile, null, maxPathfindingDepth);
        Tile closestUnitTile = null;
        foreach (Tile t in rangePaths.Keys)
        {
            if (t.type == TileType.Ally)
            {
                if (closestUnitTile == null || rangePaths[t].Count < rangePaths[closestUnitTile].Count)
                {
                    closestUnitTile = t;
                }
            }
        }
        Queue<Tile> path = new Queue<Tile>();
        foreach (Tile t in rangePaths[closestUnitTile])
        {
            path.Enqueue(t);
        }
        if (closestUnitTile != null)
        {
            path.Enqueue(closestUnitTile);
        }
        return path;
    }

    private void FindWayToUnits(Tile tile, List<Tile> previous, int endlessLoopSecurity)
    {
        List<Tile> newPrevious = new List<Tile>();
        if (tile == null || endlessLoopSecurity <= 0)
        {
            return;
        }
        if (previous != null)
        {
            if (tile.type == TileType.Ally)
            {
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
            }
            if (AbortTileCondition(tile, fullRange))
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
            if (tile.type == TileType.Ally)
            {
                return;
            }
        }
        foreach (Tile nextTile in tile.GetNeighbors())
        {
            FindWayToUnits(nextTile, newPrevious, endlessLoopSecurity--);
        }
    }

    public bool IsInRange(Tile tile)
    {
        if (tile != null && rangePaths.ContainsKey(tile))
        {
            return true;
        }
        return false;
    }

    public void AddToCurrentPath(Tile tile)
    {
        if (IsInRange(tile))
        {
            if (currentPath.Count == 0)
            {
                SetShorterCurrentPath(tile);
            }
            else
            {
                if (tile.Equals(currentPath.Peek()))
                {
                    return;
                }
                if (currentPath.Peek().type != TileType.Ally && tile.IsNeighbor(currentPath.Peek()))
                {
                    if (rangePaths[tile].Count < currentPath.Count)
                    {
                        if (currentPath.Contains(tile))
                        {
                            bool found;
                            do
                            {
                                Tile peek = currentPath.Peek();
                                found = tile.Equals(peek) || currentPath.Count == 0;
                                if (!found)
                                {
                                    currentPath.Pop().TriggerAnimation(TileAnim.Movement);
                                }
                            }
                            while (!found);
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

    public void TargetTile(Tile tile)
    {
        if (tile != null && attackPaths.ContainsKey(tile))
        {
            target = tile;
        }
        else
        {
            target = null;
        }
        DisplayAttackTiles();
        DisplayTarget();
    }

    public void SetShorterCurrentPath(Tile tile)
    {
        foreach (Tile pathTile in currentPath)
        {
            if (!rangePaths[tile].Contains(pathTile))
            {
                pathTile.TriggerAnimation(TileAnim.Movement);
            }
        }
        currentPath.Clear();
        foreach (Tile pathTile in rangePaths[tile])
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
        foreach (Tile tile in attackRange)
        {
            if (attackPaths.ContainsKey(tile))
            {
                if (target != null)
                {
                    if (attackPaths.ContainsKey(target) && attackPaths[target].Contains(tile))
                    {
                        tile.TriggerAnimation(TileAnim.AttackMouseOver);
                    }
                    else
                    {
                        tile.TriggerAnimation(TileAnim.Attack);

                    }
                }
                else
                {
                    tile.TriggerAnimation(TileAnim.Attack);
                }
            }
            else
            {
                if (target != null)
                {
                    if (attackPaths.ContainsKey(target) && attackPaths[target].Contains(tile))
                    {
                        tile.TriggerAnimation(TileAnim.AttackMouseOver);
                    }
                    else
                    {
                        tile.TriggerAnimation(TileAnim.Disabled);

                    }
                }
                else
                {
                    tile.TriggerAnimation(TileAnim.Disabled);
                }
            }
        }
    }

    public void DisplayCurrentPath()
    {
        foreach (Tile tile in currentPath)
        {
            tile.TriggerAnimation(TileAnim.MovementMouseOver);
        }
    }

    public void DisplayTarget()
    {
        if (target != null)
        {
            target.TriggerAnimation(TileAnim.AttackMouseOver);
        }
    }

    public void ClearTiles()
    {
        foreach (Tile tile in rangePaths.Keys)
        {
            tile.TriggerAnimation(TileAnim.None);
        }
        foreach (Tile tile in attackRange)
        {
            tile.TriggerAnimation(TileAnim.None);
        }
        attackRange.Clear();
        attackPaths.Clear();
        rangePaths.Clear();
        currentPath.Clear();
        target = null;
        unitTile = null;
    }

    public void ClearCurrentPath()
    {
        foreach (Tile tile in currentPath)
        {
            tile.TriggerAnimation(TileAnim.Movement);
        }
        currentPath.Clear();
    }

    public Stack<Tile> GetCurrentPath()
    {
        Stack<Tile> orderedPath = new Stack<Tile>();
        if (currentPath != null && currentPath.Count > 0)
        {
            do
            {
                orderedPath.Push(currentPath.Pop());
            }
            while (currentPath.Count > 0);
        }
        return orderedPath;
    }

    public List<Tile> GetTargets()
    {
        if (target != null && attackPaths.ContainsKey(target))
        {
            return attackPaths[target];
        }
        return new List<Tile>();
    }

}
