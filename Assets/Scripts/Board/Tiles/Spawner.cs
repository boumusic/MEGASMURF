using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Tile
{

    [HideInInspector]
    public BaseUnitType spawnedType;

    [HideInInspector]
    public bool activeSpawn;

    public int turnDelay;

    private int currentTurn;

    private void Awake()
    {
        currentTurn = 0;
        activeSpawn = true;
    }

    public void Spawn()
    {
        GameObject unitObject = UnitFactory.Instance.CreateUnit(spawnedType);
        Unit u = unitObject.GetComponent<Unit>();
        u.SpawnUnit(this);
        currentTurn = 0;
    }

    public void NewTurn()
    {
        currentTurn++;
        if (activeSpawn && currentTurn >= turnDelay && unit == null && type == TileType.Free)
        {
            Spawn();
        }
    }
}
