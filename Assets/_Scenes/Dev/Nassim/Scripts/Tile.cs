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

public enum TileAnim
{
    None,
    Movement,
    MovementMouseOver,
    Attack,
    AttackMouseOver
}

public class Tile : MonoBehaviour
{

    public TileType type;
    public Unit unit;
    public Animator animator;

    private Vector2Int _coords;

    private List<Tile> _neighbors;

    public Vector2Int Coords
    {
        get => _coords;
        set 
        {
            _coords = value;
            CheckNeighbors();
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        isProcessed = false;
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Tile> GetNeighbors()
    {
        return _neighbors;
    }

    public void CheckNeighbors()
    {
        if (_neighbors == null)
        {
            _neighbors = new List<Tile>();
        }
        else
        {
            _neighbors.Clear();
        }
        if (Board.Instance.GetTile(_coords.x + 1, _coords.y) != null)
        {
            _neighbors.Add(Board.Instance.GetTile(_coords.x + 1, _coords.y));
        }
        if (Board.Instance.GetTile(_coords.x, _coords.y + 1) != null)
        {
            _neighbors.Add(Board.Instance.GetTile(_coords.x, _coords.y + 1));
        }
        if (Board.Instance.GetTile(_coords.x - 1, _coords.y) != null)
        {
            _neighbors.Add(Board.Instance.GetTile(_coords.x - 1, _coords.y));
        }
        if (Board.Instance.GetTile(_coords.x, _coords.y - 1) != null)
        {
            _neighbors.Add(Board.Instance.GetTile(_coords.x, _coords.y - 1));
        }
    }

    public bool Equals(Tile other)
    {
        return Coords.Equals(other.Coords);
    }

    public void TriggerAnimation(TileAnim anim)
    {
        if (animator != null)
        {
            switch (anim)
            {
                case TileAnim.Movement:
                    animator.SetTrigger("Movement");
                    break;
            }
        }
    }
}
