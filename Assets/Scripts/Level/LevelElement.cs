using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all elements that will be spawned on the level (Enemies, obstacles, bonuses)
/// </summary>
public class LevelElement : Entity
{
    private Vector3 spawnPosition;
    public Vector3 SpawnPosition { get => spawnPosition; set => spawnPosition = value; }

    public LevelElement()
    {

    }

    public LevelElement(LevelElement elem)
    {
        spawnPosition = elem.spawnPosition;
    }

    public virtual Color ColorInEditor()
    {
        return Color.grey;
    }
}
