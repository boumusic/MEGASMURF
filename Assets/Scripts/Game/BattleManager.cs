﻿using System.Collections;
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

    private void Start()
    {
        gameplayState = StateMachine<GameplayState>.Initialize(this);
        gameplayState.ManualUpdate = true;
        gameplayState.ChangeState(GameplayState.UnitSelection);
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

    private void PlayerTurn_Enter()
    {
        //Anim de debut de tour
        //Abonner fin anim
        playerTurnStarted?.Invoke();
        MaestroUnit.FreshenUp();
        FreshupUnits(ShapeUnits.Cast<Unit>().ToList());
        gameplayState.ChangeState(GameplayState.UnitSelection);
    }

    private void PlayerTurnStart_Exit()
    {

    }

    private void PlayerTurnEnd_Enter()
    {

    }
    
    private void PlayerTurnEnd_Exit()
    {

    }

    private void EnemyTurnStart_Enter()
    {
        //Animation
        enemyTurnStarted?.Invoke();
        //UnExaustedEnemyUnit()
        //EnterStartPlayerTurn()
    }

    private void EnemyTurnStart_Exit()
    {

    }

    private void EnemyTurnEnd_Enter()
    {

    }

    private void EnemyTurnEnd_Exit()
    {

    }

    private void UnitSelection_Enter()
    {
        CurrentSelectedUnit = null;
        InputManager.instance.OnCancel += OpenGameplayMenu;
        InputManager.instance.OnUnitSelection += SelectUnit;

        //if (MaestroUnit.CurrentUnitState == UnitState.Used && AreAllUnitsUsed(ShapeUnits.Cast<Unit>().ToList()))
        //{
        //    gameplayState.ChangeState(GameplayState.EnemyTurnStart);
        //    return;
        //}
    }

    private void UnitSelection_Exit()
    {
        InputManager.instance.OnCancel -= OpenGameplayMenu;
        InputManager.instance.OnUnitSelection -= SelectUnit;
    }

    private void ActionSelection_Enter()
    {
        GetUnitMovementRange();
        DisplayUnitMovementRange();
        InputManager.instance.OnTileMouseOver += RangeManager.Instance.AddToCurrentPath;
        //Display la bonne UI

        InputManager.instance.OnTileSelection += OrderMovement;
        InputManager.instance.OnCancel += EnterUnitSelectionState;
    }

    private void ActionSelection_Exit()
    {
        InputManager.instance.OnTileMouseOver -= RangeManager.Instance.AddToCurrentPath;
        RangeManager.Instance.ClearTiles();
        InputManager.instance.OnTileSelection -= OrderMovement;
        InputManager.instance.OnCancel -= EnterUnitSelectionState;

        //Undisplay UI
    }

    private void MovementPseudoState_Enter()
    {
        CurrentSelectedUnit.MoveTo(movementPath);
        //Attendre la fin de l'anim
        gameplayState.ChangeState(GameplayState.AttackSelection);
    }

    private void MovementPseudoState_Exit()
    {
        tilesInMovementRange = null;
        movementPath = null;
    }

    private void AttackSelection_Enter()
    {
        InputManager.instance.OnCancel += EnterUnitSelectionState;
    }

    private void AttackSelection_Exit()
    {
        InputManager.instance.OnCancel -= EnterUnitSelectionState;
    }

    private void AttackTargetSelection_Enter()
    {
        GetUnitAttackRange();
        DisplayUnitAttackRange();
        InputManager.instance.OnTileMouseOver += RangeManager.Instance.TargetTile;
        InputManager.instance.OnTileSelection += OrderAttack;
        InputManager.instance.OnCancel += EnterAttackSelectionState;

        //Display UI
    }

    private void AttackTargetSelection_Exit()
    {
        //unDisplay UI
        InputManager.instance.OnTileMouseOver -= RangeManager.Instance.TargetTile;
        RangeManager.Instance.ClearTiles();
        InputManager.instance.OnTileSelection -= OrderAttack;
        InputManager.instance.OnCancel -= EnterAttackSelectionState;
    }

    private void AttackPseudoState_Enter()
    {
        CurrentSelectedUnit.Attack(targets);
    }

    private void AttackPseudoState_Exit()
    {
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
            if(unit.CurrentUnitState == UnitState.Fresh)
            {
                CurrentSelectedUnit = unit;
                gameplayState.ChangeState(GameplayState.ActionSelection/*AttackTargetSelection*/);                                                                                       //////LAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            }
            else if(unit.CurrentUnitState == UnitState.Moved)
            {
                CurrentSelectedUnit = unit;
                gameplayState.ChangeState(GameplayState.AttackSelection);
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

            gameplayState.ChangeState(GameplayState.AttackPseudoState);
        }
    }
}


