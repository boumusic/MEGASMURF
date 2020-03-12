using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShapeUnit : Unit
{
    [Header("Components")]
    [SerializeField] private UnitMerger unitMergeAnimator;
    [SerializeField] public Transform mergeParent;
    private Action OnMergedFinished;

    public override Tile CurrentTile
    {
        get => currentTile;
        protected set
        {
            if (currentTile != null)
            {
                currentTile.unit = null;
                currentTile.type = TileType.Free;
                currentTile = null;
            }

            if (value != null)
            {
                currentTile = value;
                currentTile.unit = this;
                currentTile.type = TileType.Ally;
                if (value is ExitTile)
                {
                    SpawnID = ((ExitTile)value).id;
                }
                else
                {
                    SpawnID = -1;
                }
            }
        }
    }

    public override BaseUnitType UnitType => BaseUnitType.ShapeComposite;

    private List<ShapeUnit> mergedUnits;
    public bool IsUnitComposite => mergedUnits.Count > 0;
    public override int UnitMergeLevel => mergedUnits.Count;

    public ShapeUnit HeadUnit => (mergedUnits.Count > 1) ? mergedUnits[1] : null;
    public ShapeUnit ArmUnit => (mergedUnits.Count > 0) ? mergedUnits[0] : null;
    public ShapeUnit LegUnit => this;

    public ShapeUnitAnimator ShapeUnitAnimator => UnitAnimator as ShapeUnitAnimator;

    public float Height => UnitMergeLevel * unitBase.unitStats.height;

    private string unitName = "";

    public override int MaxHealth
    {
        get
        {
            int maxHealth = unitBase.unitStats.maxHealth;
            
            if(mergedUnits != null)
            {
                if (mergedUnits.Count > 0)
                {
                    foreach (ShapeUnit shape in mergedUnits)
                    {
                        maxHealth += shape.MaxHealth;
                    }
                }
            }
            
            return maxHealth;
        }
    }
    public override int Damage => (ArmUnit != null) ? ArmUnit.Damage : unitBase.unitStats.damage;

    public override AttackPattern UnitAttackPattern
    {
        get
        {

            if (ArmUnit != null && unitBase.unitType == ArmUnit.UnitType)
            {
                return unitBase.attackPatterns[1];
            }
            else
            {
                return unitBase.attackPatterns[0];
            }
        }
    }

    public override MovementPattern UnitMovementPattern 
    {
        get
        {
            if (HasInfiniteMoveRange)
            {
                return unitBase.movementPatterns[unitBase.movementPatterns.Length - 1];
            }
            else
            {
                if(ArmUnit != null && unitBase.unitType == ArmUnit.UnitType)
                {
                   return unitBase.movementPatterns[1];
                }
                else
                {
                   return unitBase.movementPatterns[0];
                }
            }
        }
    }

    //public Transform MergeParent { get => mergeParent; set => mergeParent = value; }
    public Transform MergeParent => mergedUnits.Count > 1 ? mergedUnits[mergedUnits.Count - 2].mergeParent : mergeParent;

    protected override void Awake()
    {
        base.Awake();
        mergedUnits = new List<ShapeUnit>();
    }

    private ShapeUnit shapeBeingMerged;

    public override void SpawnUnit(Tile tile)
    {
        base.SpawnUnit(tile);
        BattleManager.Instance.AddUnitToPlayerUnitList(0, gameObject);
    }

    public override void MoveTo(Stack<Tile> path)
    {
        StartCoroutine(MovingTo(path, null));
        BecomeMoved();
    }

    public override void MoveTo(Stack<Tile> path, System.Action action)
    {
        // Lancer une coroutine qui fait parcourir le chemin
        StartCoroutine(MovingTo(path, action));
        BecomeMoved();
    }

    public override void TakeDamage(Unit unit)
    {
        base.TakeDamage(unit);
        ShapeUnitAnimator.PlaySpecial("SimpleHit");
    }

    private IEnumerator MovingTo(Stack<Tile> path, System.Action action)
    {
        SetAnimatorMoving(true);
        bool isMerging = false;
        TileType tempType = TileType.Free;
        
        if (currentTile != null)
        {
            currentTile.unit = null;
            tempType = currentTile.type;
            currentTile.type = TileType.Free;
        }
        while (path.Count > 0)
        {
            if (path.Count == 1)
            {
                if (path.Peek().type == TileType.Ally)
                {
                    (path.Pop().unit as ShapeUnit).InitiateMergeAlly(this as ShapeUnit, SequenceManager.Instance.Resume);
                    currentTile = null;
                    isMerging = true;
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

        if (currentTile != null)
        {
            currentTile.unit = this;
            currentTile.type = tempType;
        }

        SetAnimatorMoving(false);
        FaceCamera();

        BecomeMoved();

        if (!isMerging)
            action?.Invoke();
    }

    public void InitiateMergeAlly(ShapeUnit shape)
    {
        InitiateMergeAlly(shape, null);
    }

    /// <summary>
    /// Takes a unit and merges it on top of itself.
    /// </summary>
    /// <param name="shape">The shape that will be merged on top of this shape.</param>
    public void InitiateMergeAlly(ShapeUnit shape, System.Action onFinishedAdditionalAction)
    {
        if (shape.UnitMergeLevel == 0)
        {
            if (UnitMergeLevel < 2)
            {
                OnMergedFinished += FinishedMerging;
                OnMergedFinished += ResetHealth;
                OnMergedFinished += onFinishedAdditionalAction;
                shapeBeingMerged = shape;
                mergedUnits.Add(shape);
                shape.unitMergeAnimator.MergeOnTopOf(this, OnMergedFinished);
                BattleManager.Instance.RemoveUnitFromPlay(mergedUnits[mergedUnits.Count - 1]);
            }
            else
                Debug.LogError("Illicite Merge: bottom unit is already at max level");
        }
        else
            Debug.LogError("Illicite Merge: intiating unit is not level 0");

    }

    public void ToggleMembers(ShapeUnit destination)
    {
        destination.ShapeUnitAnimator.ToggleArms(false);
        destination.ShapeUnitAnimator.ToggleFace(false);

        int count = destination.mergedUnits.Count;

        ShapeUnitAnimator.ToggleLegs(false);

        if (count == 2)
        {
            destination.mergedUnits[0].ShapeUnitAnimator.ToggleFace(false);
            ShapeUnitAnimator.ToggleArms(false);
        }
    }

    public override void SetAnimatorMoving(bool moving)
    {
        base.SetAnimatorMoving(moving);
        for (int i = 0; i < mergedUnits.Count; i++)
        {
            mergedUnits[i].SetAnimatorMoving(moving);
        }
    }

    private void FinishedMerging()
    {
        if (shapeBeingMerged)
        {
            shapeBeingMerged = null;
        }
        // Autre check 
        // Vanish d'equipement + Refund
    }

    public void NonGampelayMerge(ShapeUnit shape)
    {
        if (shape.UnitMergeLevel == 0)
        {
            if (UnitMergeLevel < 2)
                mergedUnits.Add(shape);
            else
                Debug.LogError("Illicite Merge: bottom unit is already at max level");
        }
        else
            Debug.LogError("Illicite Merge: intiating unit is not level 0");

    }
}
