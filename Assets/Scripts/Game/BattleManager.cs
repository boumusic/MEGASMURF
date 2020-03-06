using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System.Linq;

public enum GameplayState
{
    LevelStart,
    PlayerTurnStart,
    PlayerTurnEnd,
    EnemyTurnStart,
    EnemyTurnEnd,
    UnitSelection,
    ActionSelection,
    MovementPseudoState,
    AttackSelection,
    AttackTargetSelection,
    AttackPseudoState
}

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance { get { if (!instance) instance = FindObjectOfType<BattleManager>(); return instance; } }

    public bool debugMode;
    public GameObject[] debugStartingUnits;

    public delegate void GameEvent();
    public GameEvent playerTurnStarted;
    public GameEvent enemyTurnStarted;

    [Header("Players")]
    [SerializeField] private HumanPlayer humanPlayer;
    [SerializeField] private AIPlayer aiPlayer;

    private StateMachine<GameplayState> gameplayState;


    public Maestro MaestroUnit { get; private set; }
    public List<ShapeUnit> ShapeUnits { get; private set; }
    public List<Enemy> Enemies { get; private set; }

    public Unit CurrentSelectedUnit { get; private set; }
    private List<Tile> tilesInMovementRange;
    private Stack<Tile> movementPath;
    private List<Tile> tilesInAttackRange;
    private List<Tile> targets;

    private void Awake()
    {
        ShapeUnits = new List<ShapeUnit>();
        Enemies = new List<Enemy>();

        Board.Instance.InitializeBoard();
    }

    private void Start()
    {
        gameplayState = StateMachine<GameplayState>.Initialize(this);
        gameplayState.ManualUpdate = true;

        if (debugMode)
        {
            FillUnitLists(debugStartingUnits);
            DebugSetupAllUnits();
        }

        gameplayState.ChangeState(GameplayState.PlayerTurnStart);
    }

    public void StartLevel()
    {

    }

    public void OpenGameplayMenu()
    {
        //OpenUi
        //Sauvegarder le delegate de cancel
        //abonner closeGameplayMenu sur cancel
    }

    public void CloseGameplayMenu()
    {
        //Close UI
        //Remettre l'ancien delegate de cancel dans cancel
    }

    public void PlayerEndTurn()
    {
        gameplayState.ChangeState(GameplayState.EnemyTurnStart);
    }

    public void EnterUnitSelectionState()
    {
        gameplayState.ChangeState(GameplayState.UnitSelection);
    }

    public void EnterAppropriateActionState()
    {
        if (CurrentSelectedUnit.CurrentUnitState == UnitState.Fresh)
        {
            gameplayState.ChangeState(GameplayState.ActionSelection);                                                                                       
        }
        else if (CurrentSelectedUnit.CurrentUnitState == UnitState.Moved)
        {
            gameplayState.ChangeState(GameplayState.AttackSelection);
        }
    }

    public void EnterActionSelectionState()
    {
        gameplayState.ChangeState(GameplayState.ActionSelection);
    }

    public void EnterAttackSelectionState()
    {
        gameplayState.ChangeState(GameplayState.AttackSelection);
    }

    public void EnterAttackTargetSelectionState()
    {
        gameplayState.ChangeState(GameplayState.AttackTargetSelection);
    }

    #region State
    private void LevelStart_Enter()
    {

    }

    private void LevelStart_Exit()
    {

    }

    private void PlayerTurnStart_Enter()
    {
        Debug.Log("Enter PlayerTurnStart State!");
        //Anim de debut de tour
        //Abonner fin anim
        playerTurnStarted?.Invoke();
        MaestroUnit?.FreshenUp();
        FreshupUnits(ShapeUnits.Cast<Unit>().ToList());
        gameplayState.ChangeState(GameplayState.UnitSelection);
    }

    private void PlayerTurnStart_Exit()
    {
        Debug.Log("Exit PlayerTurnStart State!");
    }

    private void PlayerTurnEnd_Enter()
    {
        Debug.Log("Enter PlayerTurnEnd State!");
        gameplayState.ChangeState(GameplayState.EnemyTurnStart);
    }

    private void PlayerTurnEnd_Exit()
    {
        Debug.Log("Exit PlayerTurnEnd State!");

    }

    private void EnemyTurnStart_Enter()
    {
        Debug.Log("Enter EnemyTurnStart State!");
        //Animation
        enemyTurnStarted?.Invoke();
        FreshupUnits(Enemies.Cast<Unit>().ToList());
        gameplayState.ChangeState(GameplayState.EnemyTurnEnd);
    }

    private void EnemyTurnStart_Exit()
    {
        Debug.Log("Exit EnemyTurnStart State!");

    }

    private void EnemyTurnEnd_Enter()
    {
        Debug.Log("Enter EnemyTurnEnd State!");
        gameplayState.ChangeState(GameplayState.PlayerTurnStart);
    }

    private void EnemyTurnEnd_Exit()
    {
        Debug.Log("Exit EnemyTurnEnd State!");
    }

    private void UnitSelection_Enter()
    {
        Debug.Log("Enter UnitSelection State!");
        CurrentSelectedUnit = null;
        InputManager.instance.OnCancel += OpenGameplayMenu;
        InputManager.instance.OnUnitSelection += SelectUnit;

        if (/*MaestroUnit.CurrentUnitState == UnitState.Used &&*/ AreAllUnitsUsed(ShapeUnits.Cast<Unit>().ToList()))
        {
            gameplayState.ChangeState(GameplayState.PlayerTurnEnd);
            return;
        }
    }

    private void UnitSelection_Exit()
    {
        Debug.Log("Exit UnitSelection State!");
        InputManager.instance.OnCancel -= OpenGameplayMenu;
        InputManager.instance.OnUnitSelection -= SelectUnit;
    }

    private void ActionSelection_Enter()
    {
        Debug.Log("Enter ActionSelection State!");
        GetUnitMovementRange();
        DisplayUnitMovementRange();
        InputManager.instance.OnTileMouseOver += RangeManager.Instance.AddToCurrentPath;
        //Display la bonne UI

        InputManager.instance.OnTileSelection += OrderMovement;
        InputManager.instance.OnAttackButtonPress += EnterAttackTargetSelectionState;
        InputManager.instance.OnCancel += EnterUnitSelectionState;
    }

    private void ActionSelection_Exit()
    {
        Debug.Log("Exit ActionSelection State!");
        InputManager.instance.OnTileMouseOver -= RangeManager.Instance.AddToCurrentPath;
        RangeManager.Instance.ClearTiles();
        InputManager.instance.OnTileSelection -= OrderMovement;
        InputManager.instance.OnAttackButtonPress -= EnterAttackTargetSelectionState;
        InputManager.instance.OnCancel -= EnterUnitSelectionState;

        //Undisplay UI
    }

    private void MovementPseudoState_Enter()
    {
        Debug.Log("Enter MovementPseudoState State!");
        CurrentSelectedUnit.MoveTo(movementPath);
        //Attendre la fin de l'anim
        gameplayState.ChangeState(GameplayState.AttackSelection);
    }

    private void MovementPseudoState_Exit()
    {
        Debug.Log("Exit MovementPseudoState State!");
        tilesInMovementRange = null;
        movementPath = null;
    }

    private void AttackSelection_Enter()
    {
        Debug.Log("Enter AttackSelection State!");
        InputManager.instance.OnAttackButtonPress += EnterAttackTargetSelectionState;
        InputManager.instance.OnCancel += EnterUnitSelectionState;
    }

    private void AttackSelection_Exit()
    {
        Debug.Log("Exit AttackSelection State!");
        InputManager.instance.OnAttackButtonPress -= EnterAttackTargetSelectionState;
        InputManager.instance.OnCancel -= EnterUnitSelectionState;
    }

    private void AttackTargetSelection_Enter()
    {
        Debug.Log("Enter AttackTargetSelection State!");
        GetUnitAttackRange();
        DisplayUnitAttackRange();
        InputManager.instance.OnTileMouseOver += RangeManager.Instance.TargetTile;
        InputManager.instance.OnTileSelection += OrderAttack;
        InputManager.instance.OnCancel += EnterAppropriateActionState;

        //Display UI
    }

    private void AttackTargetSelection_Exit()
    {
        Debug.Log("Exit AttackTargetSelection State!");
        //unDisplay UI
        InputManager.instance.OnTileMouseOver -= RangeManager.Instance.TargetTile;
        RangeManager.Instance.ClearTiles();
        InputManager.instance.OnTileSelection -= OrderAttack;
        InputManager.instance.OnCancel -= EnterAppropriateActionState;
    }

    private void AttackPseudoState_Enter()
    {
        Debug.Log("Enter AttackPseudoState State!");
        CurrentSelectedUnit.Attack(targets);
        gameplayState.ChangeState(GameplayState.UnitSelection);
    }

    private void AttackPseudoState_Exit()
    {
        Debug.Log("Exit AttackPseudoState State!");
        tilesInAttackRange = null;
        targets = null;
    }
    #endregion

    private void FreshupUnits(List<Unit> units)
    {
        foreach(Unit unit in units)
        {
            unit.FreshenUp();
        }
    }

    private bool AreAllUnitsUsed(List<Unit> units)
    {
        foreach(Unit unit in units)
        {
            if (unit.CurrentUnitState != UnitState.Used)
                return false;
        }
        return true;
    }

    private void SelectUnit(Unit unit)
    {
        if(unit is ShapeUnit)
        {
            if (unit.CurrentUnitState == UnitState.Fresh || unit.CurrentUnitState == UnitState.Moved)
            {
                CurrentSelectedUnit = unit;
                EnterAppropriateActionState();
            }
        }
    }

    private void GetUnitMovementRange()
    {
        tilesInMovementRange = RangeManager.Instance.GetTilesInMovementRange(CurrentSelectedUnit.CurrentTile);
    }

    private void DisplayUnitMovementRange()
    {
        RangeManager.Instance.DisplayMovementTiles();
    }

    private void OrderMovement(Tile tile)
    {
        if(tilesInMovementRange.Contains(tile))
        {
            movementPath = RangeManager.Instance.GetCurrentPath();

            gameplayState.ChangeState(GameplayState.MovementPseudoState);
        }
    }

    private void GetUnitAttackRange()
    {
        tilesInAttackRange = RangeManager.Instance.GetTilesInAttackRange(CurrentSelectedUnit.CurrentTile);
    }

    private void DisplayUnitAttackRange()
    {
        RangeManager.Instance.DisplayAttackTiles();
    }

    private void OrderAttack(Tile tile)
    {
        if(tilesInAttackRange.Contains(tile))
        {
            targets = RangeManager.Instance.GetTargets();
            if (CurrentSelectedUnit.UnitAttackPattern.type == AttackPatternType.Slice)
                targets.Add(tile);
            gameplayState.ChangeState(GameplayState.AttackPseudoState);
        }
    }

    private void FillUnitLists(GameObject[] startingUnits)
    {
        foreach(GameObject unitGameObject in startingUnits)
        {
            Unit unit;
            if ((unit = unitGameObject.GetComponent<Maestro>()) != null)
                MaestroUnit = (Maestro)unit;
            else if ((unit = unitGameObject.GetComponent<ShapeUnit>()) != null)
                ShapeUnits.Add((ShapeUnit)unit);
            else if ((unit = unitGameObject.GetComponent<Enemy>()) != null)
                Enemies.Add((Enemy)unit);
        }
    }

    private void DebugSetupAllUnits()
    {
        MaestroUnit?.DebugSetUnitPosition();

        foreach(ShapeUnit shape in ShapeUnits)
        {
            shape.DebugSetUnitPosition();
        }

        foreach(Enemy enemy in Enemies)
        {
            enemy.DebugSetUnitPosition();
        }
    }
}


