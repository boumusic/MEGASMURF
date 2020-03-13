using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pataya.QuikFeedback;

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
    AttackMouseOver,
    Summon,
    Disabled,
    EnemyMovement
}

public class Tile : LevelElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TileType type;
    public Unit unit;
    public Animator animator;
    [Header("Feedbacks")]
    public QuikFeedback attackedByShape;
    public QuikFeedback attackedByEnemy;

    public Renderer rend;

    public ParticleSystem mudFXPop;

    private bool isAppeared = false;

    [Header("UI")]
    public GameObject hovered;

    public int MudAmount { get; set; }

    private Vector2 _coords;

    private List<Tile> _neighbors;

    private TileAnim _currentAnim;

    public Vector2 Coords
    {
        get => _coords;
        set
        {
            _coords = value;
            CheckNeighbors();
        }
    }

    public void SetMaterial(Material mat)
    {
        if(rend != null)
        {
            rend.material = mat;
        }
    }

    public void ReceiveAttack(Unit unit)
    {
        if (unit is Enemy)
        {
            attackedByEnemy.Play();
        }

        else if (unit is ShapeUnit)
        {
            attackedByShape.Play();
        }
    }

    private void Awake()
    {
        MudAmount = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        _currentAnim = TileAnim.None;
    }

    private void OnEnable()
    {
        hovered.SetActive(false);
    }

    private void Update()
    {
        if (MudAmount <= 0)
        {
            mudFXPop.Clear();
            mudFXPop.Stop();
        }
    }

    public override void Appear()
    {
        if (!isAppeared)
        {
            animator.SetTrigger("In");
            isAppeared = true;

            AudioManager.Instance.PlaySFX("Pop");
        }
    }

    public void ResetAppeared() 
    {
        isAppeared = false;
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
        if (Board.Instance.GetTile((int)_coords.x + 1, (int)_coords.y) != null)
        {
            _neighbors.Add(Board.Instance.GetTile((int)_coords.x + 1, (int)_coords.y));
        }
        if (Board.Instance.GetTile((int)_coords.x, (int)_coords.y + 1) != null)
        {
            _neighbors.Add(Board.Instance.GetTile((int)_coords.x, (int)_coords.y + 1));
        }
        if (Board.Instance.GetTile((int)_coords.x - 1, (int)_coords.y) != null)
        {
            _neighbors.Add(Board.Instance.GetTile((int)_coords.x - 1, (int)_coords.y));
        }
        if (Board.Instance.GetTile((int)_coords.x, (int)_coords.y - 1) != null)
        {
            _neighbors.Add(Board.Instance.GetTile((int)_coords.x, (int)_coords.y - 1));
        }
    }

    public bool IsNeighbor(Tile other)
    {
        return Vector2.Distance(Coords, other.Coords) == 1.0f;
    }

    public bool IsInLine(Tile other)
    {
        return Coords.x == other.Coords.x || Coords.y == other.Coords.y || Mathf.Abs(Coords.x - other.Coords.x) == Mathf.Abs(Coords.y - other.Coords.y);
    }

    public bool Equals(Tile other)
    {
        return Coords.Equals(other.Coords);
    }

    public void TriggerAnimation(TileAnim anim)
    {
        if (animator != null)// && _currentAnim != anim)
        {
            /*
            switch (anim)
            {
                case TileAnim.Movement:
                    animator.SetTrigger("Movement");
                    break;
                case TileAnim.MovementMouseOver:
                    animator.SetTrigger("MovementMouseOver");
                    break;
                case TileAnim.Attack:
                    animator.SetTrigger("Attack");
                    break;
                case TileAnim.AttackMouseOver:
                    animator.SetTrigger("AttackMouseOver");
                    break;
                case TileAnim.Summon:
                    animator.SetTrigger("Summon");
                    break;
                case TileAnim.Disabled:
                    animator.SetTrigger("Disabled");
                    break;
                default:
                    animator.SetTrigger("None");
                    break;
            }
            */

            _currentAnim = anim;
            animator.SetTrigger(anim.ToString());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Animation Si on est dans le bon State de BattleManager
        InputManager.instance.UpdateCurrentTile(this);
        if (unit != null)
        {
            if (type == TileType.Enemy && BattleManager.Instance.CurrentPlayerID == 0 && (RangeManager.Instance.unitTile == null || RangeManager.Instance.unitTile.type == TileType.Enemy))
            {
                RangeManager.Instance.ClearTiles();
                RangeManager.Instance.GetTilesInMovementRange(this);
                RangeManager.Instance.DisplayMovementTiles();
                //BattleManager.Instance.DisplayUnitMovementRange();
            }
        }
        hovered.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if (BattleManager.Instance.CurrentPlayerID == 0 && RangeManager.Instance.unitTile != null && RangeManager.Instance.unitTile.type == TileType.Enemy)
        {
           RangeManager.Instance.ClearTiles();
        }
        hovered.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (unit != null)
        {
            if (type == TileType.Enemy && BattleManager.Instance.CurrentPlayerID == 0 && (RangeManager.Instance.unitTile == null || RangeManager.Instance.unitTile.type == TileType.Enemy))
            {
                RangeManager.Instance.ClearTiles();
                RangeManager.Instance.GetTilesInAttackRange(this);
                RangeManager.Instance.DisplayAttackTiles();
                //BattleManager.Instance.DisplayUnitActionRange();
            }
        }
        InputManager.instance.TileClickCallBack(this);
    }
}
