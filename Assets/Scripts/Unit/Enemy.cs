using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [HideInInspector]
    public bool activated;
    public int calls;

    protected override void Awake()
    {
        base.Awake();
        activated = false;
        calls = 0;
    }

    public override Tile CurrentTile
    {
        get => currentTile;
        protected set
        {
            if (currentTile != null)
            {
                currentTile.unit = null;
                currentTile.type = TileType.Free;
                currentTile = null;
            }

            if (value != null)
            {
                currentTile = value;
                currentTile.unit = this;
                currentTile.type = TileType.Enemy;
            }
        }
    }

    public void Sequence()
    {
        if (!activated)
        {
            calls++;
            activated = true;
            if (CurrentUnitState == UnitState.Fresh)
            {
                CallAttack();
            }
            if (CurrentUnitState == UnitState.Fresh)
            {
                Stack<Tile> path = FindClosestEnemyPath();
                if (path != null && path.Count > 0)
                {
                    MoveTo(path, CallAttack);
                }
            }
        }
    }

    public void CallAttack()
    {
        List<Tile> targets = FindEnemiesInRange();
        if (targets.Count > 0)
        {
            Attack(targets);
        }
    }

    public List<Tile> FindEnemiesInRange()
    {
        List<Tile> targets = new List<Tile>();
        if (unitBase.unitType == BaseUnitType.Patrolio)
        {
            List<Tile> upLine = Board.Instance.GetTilesInLine(currentTile, Direction.Up);
            List<Tile> downLine = Board.Instance.GetTilesInLine(currentTile, Direction.Down);
            List<Tile> leftLine = Board.Instance.GetTilesInLine(currentTile, Direction.Left);
            List<Tile> rightLine = Board.Instance.GetTilesInLine(currentTile, Direction.Right);

            List<Tile> tempLine = new List<Tile>();
            foreach(Tile t in upLine)
            {
                if(t.type == TileType.Ally)
                {
                    tempLine.Add(t);
                }
            }
            upLine.Clear();
            upLine.AddRange(tempLine);
            targets = upLine;
            tempLine.Clear();
            foreach (Tile t in downLine)
            {
                if (t.type == TileType.Ally)
                {
                    tempLine.Add(t);
                }
            }
            downLine.Clear();
            downLine.AddRange(tempLine);
            if (downLine.Count > targets.Count)
            {
                targets = downLine;
            }
            tempLine.Clear();
            foreach (Tile t in leftLine)
            {
                if (t.type == TileType.Ally)
                {
                    tempLine.Add(t);
                }
            }
            leftLine.Clear();
            leftLine.AddRange(tempLine);
            if (leftLine.Count > targets.Count)
            {
                targets = leftLine;
            }
            tempLine.Clear();
            foreach (Tile t in rightLine)
            {
                if (t.type == TileType.Ally)
                {
                    tempLine.Add(t);
                }
            }
            rightLine.Clear();
            rightLine.AddRange(tempLine);
            if (rightLine.Count > targets.Count)
            {
                targets = rightLine;
            }
            tempLine.Clear();

            return targets;
        }
        foreach (Vector2 v in UnitAttackPattern.range.coords)
        {
            Tile t = Board.Instance.GetTile(v + CurrentTile.Coords);
            if (t != null && t.type == TileType.Ally)
            {
                targets.Add(t);
            }
        }
        if(unitBase.unitType == BaseUnitType.Bombi)
        {
            foreach(Tile t in targets)
            {
                if(Vector2.Distance(t.Coords, currentTile.Coords) < 2)
                {
                    return targets;
                }
            }
            return new List<Tile>();
        }
        return targets;
    }

    public Stack<Tile> FindClosestEnemyPath()
    {
        if (UnitMovementPattern == null)
        {
            return null;
        }
        Unit closestUnit = null;
        Stack<Tile> destination = new Stack<Tile>();
        if (UnitMovementPattern.type == MovementPatternType.Teleport)
        {
            foreach (Unit u in BattleManager.Instance.playerUnits[0])
            {
                if (closestUnit != null)
                {
                    if (Vector2.Distance(closestUnit.CurrentTile.Coords, CurrentTile.Coords) > Vector2.Distance(u.CurrentTile.Coords, CurrentTile.Coords))
                    {
                        closestUnit = u;
                    }
                }
                else
                {
                    closestUnit = u;
                }
            }
            if (closestUnit != null)
            {
                foreach (Vector2 v in UnitMovementPattern.range.coords)
                {
                    Tile t = Board.Instance.GetTile(v + CurrentTile.Coords);
                    if (t != null && (destination.Count == 0 || Vector2.Distance(t.Coords, closestUnit.CurrentTile.Coords) < Vector2.Distance(destination.Peek().Coords, closestUnit.CurrentTile.Coords)))
                    {
                        while (destination.Count > 0)
                        {
                            destination.Pop();
                        }
                        destination.Push(t);
                    }
                }
            }
        }
        else if (UnitMovementPattern.type == MovementPatternType.Walk)
        {
            Stack<Tile> path = RangeManager.Instance.AIPathfinding(CurrentTile);
            while (path.Count > 0)
            {
                if(UnitMovementPattern.range.coords.Contains(path.Peek().Coords - CurrentTile.Coords))
                {
                    destination.Push(path.Pop());
                }
                else
                {
                    path.Pop();
                }
            }
        }
        return destination;
    }

    protected override void Die()
    {
        if (BattleManager.Instance.IsCurrentPlayerUnit(this))
            AIManager.instance.AIDeathCallBack();
        base.Die();
        
    }

    public override Color ColorInEditor()
    {
        return base.ColorInEditor();
    }
}
