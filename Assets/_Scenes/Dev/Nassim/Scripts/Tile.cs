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
    public Unit unit;
    public Animator animator;

    [HideInInspector]
    public bool isProcessed;

    private Vector2 _coords;

    public List<Tile> Neighbors { get; set; }
    public Vector2 Coords
    {
        get => _coords;
        set 
        {
            _coords = value;
            if(Neighbors == null)
            {
                Neighbors = new List<Tile>();
            }
            Neighbors.Clear();
            if(Board.Instance.GetTile(value.x + 1, value.y) != null)
            {
                Neighbors.Add(Board.Instance.GetTile(value.x + 1, value.y));
            }
            if (Board.Instance.GetTile(value.x, value.y + 1) != null)
            {
                Neighbors.Add(Board.Instance.GetTile(value.x, value.y + 1));
            }
            if (Board.Instance.GetTile(value.x - 1, value.y) != null)
            {
                Neighbors.Add(Board.Instance.GetTile(value.x - 1, value.y));
            }
            if (Board.Instance.GetTile(value.x, value.y - 1) != null)
            {
                Neighbors.Add(Board.Instance.GetTile(value.x, value.y - 1));
            }
        } }

    // Start is called before the first frame update
    void Start()
    {
        isProcessed = false;
        Neighbors = new List<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Equals(Tile other)
    {
        return Coords.Equals(other.Coords);
    }
}
