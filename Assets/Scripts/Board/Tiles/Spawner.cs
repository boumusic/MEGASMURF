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

    [Header("Spawner")]
    public ParticleSystem[] levelVfx;

    private void Awake()
    {
        currentTurn = 0;
        activeSpawn = true;
        UpdateVFX();
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
        UpdateVFX();

        if (activeSpawn && currentTurn >= turnDelay && unit == null && type == TileType.Free)
        {
            Spawn();
        }
    }

    private void UpdateVFX()
    {
        for (int i = 0; i < levelVfx.Length; i++)
        {
            levelVfx[i].gameObject.SetActive(false);
        }

        levelVfx[currentTurn].gameObject.SetActive(true);
    }
}
