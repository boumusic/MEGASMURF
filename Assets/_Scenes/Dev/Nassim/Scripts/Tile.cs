using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    None,
    Free,
    Obstacle,
    Ally,
    Enemy
}

public class Tile : MonoBehaviour
{

    public TileType type;
    //public Unit unit;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
