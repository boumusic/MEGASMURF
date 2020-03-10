using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    Fresh,
    Moved,
    Used
}

public abstract class Unit : LevelElement
{
    [Header("Components")]
    [SerializeField] private UnitAnimator unitAnimator;
    [SerializeField] private Jauge hp;

    public Vector2 debugTile;

    public UnitBase unitBase;     //Passage en UnitBase

    protected Tile currentTile;
    public virtual Tile CurrentTile { get; protected set; }
    
    public virtual BaseUnitType UnitType => unitBase.unitType;

    public UnitState CurrentUnitState { get; private set; }

    public virtual int UnitMergeLevel => 0;

    //A Initialiser
    public float CurrentHitPoint { get; protected set; }

    public virtual int MaxHealth => unitBase.unitStats.maxHealth;
    public virtual int Damage => unitBase.unitStats.damage;

    public virtual AttackPattern UnitAttackPattern => unitBase.attackPatterns[0];
    public virtual MovementPattern UnitMovementPattern => unitBase.movementPatterns[0];
    public virtual UnitStatistics UnitStats => unitBase.unitStats;

    public UnitAnimator UnitAnimator { get => unitAnimator; }

    protected virtual void Awake()
    {
        currentTile = null;
        ResetHealth();
    }

    public virtual void Start()
    {
        FaceCamera();
        if(hp)
            hp.UpdateJauge(CurrentHitPoint, MaxHealth);
    }

    public virtual void SetUnitPosition(Tile tile)
    {
        CurrentTile = tile;
        transform.position = tile.transform.position;
    }

    public void RemoveFromBoard()
    {
        CurrentTile = null;
    }

    public void DebugSetUnitPosition()
    {
        SetUnitPosition(Board.Instance.GetTile(debugTile));
        ResetHealth();
    }

    public virtual void ResetHealth()
    {
        CurrentHitPoint = MaxHealth;
    }

    public virtual void FreshenUp()
    {
        CurrentUnitState = UnitState.Fresh;
        //if stunned => Used State? ou state <-> Stunned + 1
        //if Used => Fresh
    }

    public virtual void MoveTo(Stack<Tile> path)
    {
        StartCoroutine(MovingTo(path, null));
        BecomeMoved();
    }

    public virtual void MoveTo(Stack<Tile> path, System.Action action)
    {
        // Lancer une coroutine qui fait parcourir le chemin
        StartCoroutine(MovingTo(path, action));
        BecomeMoved();
    }

    private IEnumerator MovingTo(Stack<Tile> path, System.Action action)
    {
        SetAnimatorMoving(true);

        TileType tempType = TileType.Free;

        if(currentTile != null) 
        {
            currentTile.unit = null;
            tempType = currentTile.type;
            currentTile.type = TileType.Free;
        }
        while (path.Count > 0)
        {
            if(path.Count == 1)
            {
                if(path.Peek().type == TileType.Ally)
                {
                    (path.Pop().unit as ShapeUnit).InitiateMergeAlly(this as ShapeUnit);
                    currentTile = null;
                    break;
                }
            }

            Tile destinationTile = path.Pop();
            Vector3 pos = destinationTile.transform.position;
            transform.forward = (pos - transform.position).normalized;
            while (transform.position != pos)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, UnitStats.moveSpeed);
                yield return new WaitForFixedUpdate();
            }
            CurrentTile = destinationTile;
        }

        if(currentTile != null) {
            currentTile.unit = this;
            currentTile.type = tempType;
        }

        SetAnimatorMoving(false);
        FaceCamera();
        action?.Invoke();
    }

    public virtual void SetAnimatorMoving(bool moving)
    {
        unitAnimator.SetIsMoving(moving);
    }

    public void FaceCamera()
    {
        Vector3 forward = GameCamera.Instance.Forward;
        transform.forward = new Vector3(-forward.x, 0f, -forward.z);
    }

    public virtual void Attack(List<Tile> tiles)
    {
        switch (UnitAttackPattern.type)
        {
            case AttackPatternType.All:
                foreach (Tile tile in tiles)
                {
                    if (tile.unit != null)
                        tile.unit.TakeDamage(this);
                }
                break;

            case AttackPatternType.Single:
                if (tiles.Count > 0 && tiles[0].unit != null)
                    tiles[0].unit.TakeDamage(this);
                break;

            case AttackPatternType.Slice:                                                       //Bricolage a reprendre avec les anims (sequenceur)
                Stack<Tile> attackDestination = new Stack<Tile>();
                if(tiles.Count > 1) 
                {
                    attackDestination.Push(tiles[tiles.Count - 1]);
                }

                List<Tile> unitTiles = new List<Tile>();
                foreach (Tile tile in tiles)
                {
                    if (tile.unit != null)
                        unitTiles.Add(tile);
                }

                StartCoroutine(MovingTo(attackDestination, null));
                
                foreach(Tile tile in unitTiles)
                {
                    tile.unit.TakeDamage(this);
                }
                break;
        }

        BecomeExhausted();
    }

    /// <summary>
    /// Method to receive damage
    /// </summary>
    /// <param name="unit">Unit who inflict the damage</param>
    public virtual void TakeDamage(Unit unit)
    {
        Debug.Log(gameObject.name + " took " + unit.Damage + " damage from " + unit.gameObject.name);
        CurrentHitPoint -= unit.Damage;
        Debug.Log("He now has " + CurrentHitPoint);
        if(hp)hp.UpdateJauge(CurrentHitPoint, MaxHealth);

        if (CurrentHitPoint <= 0)
        {
            Die();
            unit.OnKillScored(this);
        }
    }

    /// <summary>
    /// Callback method to enable killer specific behaviour (like taking dead unit position)
    /// </summary>
    /// <param name="unit"> Killed unit </param>
    protected virtual void OnKillScored(Unit unit)
    {

    }

    /// <summary>
    /// Execute all the action needed upon unit death
    /// </summary>
    protected virtual void Die()
    {
        BattleManager.Instance.RemoveUnitFromPlay(this);
        //animation
        gameObject.SetActive(false);
    }

    public virtual void BecomeMoved()
    {
        CurrentUnitState = UnitState.Moved;
    }

    public virtual void BecomeExhausted()
    {
        CurrentUnitState = UnitState.Used;
    }
}
