using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    public Unit BrainsUnit { get; private set; }

    public List<Unit> UnitInRange { get; }

    public List<Unit> KillableUnitInRange { get; }

    private Intention intention;

    public Brain(Enemy enemy/*, AIBehaviour behaviour*/)
    {
        BrainsUnit = enemy;
        
        //SetupAIBehaviour(behaviour);
    }

    public void SetBrainsUnit(Enemy enemy)
    {
        
        enemy.UnitBrain = this;
    }

    public Intention DefineIntention()
    {
        throw new NotImplementedException();
    }

    public void Execute()
    {
        //execute les intentions
    }

    public List<Tile> FindEnemiesInRange()
    {
        List<Tile> targets = new List<Tile>();
        foreach(Vector2 v in BrainsUnit.UnitAttackPattern.range.coords)
        {
            Tile t = Board.Instance.GetTile(v + BrainsUnit.CurrentTile.Coords);
            if(t != null && t.type == TileType.Ally)
            {
                targets.Add(t);
            }
        }
        return targets;
    }

    public Stack<Tile> FindClosestEnemyPath()
    {
        if (BrainsUnit == null || BrainsUnit.UnitMovementPattern == null)
        {
            return null;
        }
        Unit closestUnit = null;
        Stack<Tile> destination = new Stack<Tile>();
        if (BrainsUnit.UnitMovementPattern.type == MovementPatternType.Teleport)
        {
            foreach (Unit u in GameManager.units)
            {
                if (closestUnit != null)
                {
                    if (Vector2.Distance(closestUnit.CurrentTile.Coords, BrainsUnit.CurrentTile.Coords) > Vector2.Distance(u.CurrentTile.Coords, BrainsUnit.CurrentTile.Coords))
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
                foreach (Vector2 v in BrainsUnit.UnitMovementPattern.range.coords)
                {
                    Tile t = Board.Instance.GetTile(v + BrainsUnit.CurrentTile.Coords);
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
        else if (BrainsUnit.UnitMovementPattern.type == MovementPatternType.Walk)
        {
            Stack<Tile> path = RangeManager.Instance.AIPathfinding(BrainsUnit.CurrentTile);
            if (path.Count > 0)
            {
                closestUnit = path.Pop().unit;
            }
            while (path.Count > 0 && BrainsUnit.UnitMovementPattern.range.coords.Contains(destination.Peek().Coords - BrainsUnit.CurrentTile.Coords))
            {
                destination.Push(path.Pop());
            }
        }
        return destination;
    }

    public float EvaluateIntentionSafeness(Intention intention)
    {
        throw new NotImplementedException();
    }

    public List<Tile> FindShortestPathTo(Tile target)
    {
        throw new NotImplementedException();
    }
}
