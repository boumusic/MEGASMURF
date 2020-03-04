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
        AIManager.instance.AddBrain(this);
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

    public Unit FindClosestEnemy()
    {
        throw new NotImplementedException();
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
