using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance { get { if (!instance) instance = FindObjectOfType<BattleManager>(); return instance; } }

    [Header("Debug Settings")]
    public bool debugMode;
    public GameObject[] debugPlayer1StartingUnits;
    public GameObject[] debugPlayer2StartingUnits;
    public int debugStartingPlayer;

    [Header("Players Settings")]
    public Player[] players;
    public List<List<Unit>> playerUnits;

    private int StartingPlayer { get; set; }
    public Player CurrentPlayer => (CurrentPlayerID < players.Length) ? players[CurrentPlayerID] : null;
    public int CurrentPlayerID { get; private set; }

    public event Action OnPlayerTurnStart;


    public Unit CurrentSelectedUnit { get; private set; }
    private List<Tile> tilesInMovementRange;
    private Stack<Tile> movementPath;
    private List<Tile> tilesInAttackRange;
    private List<Tile> targets;
    private bool isMerging;

    private void Awake()
    {
        playerUnits = new List<List<Unit>>();
        for(int i = 0; i < players.Length; i++)
        {
            playerUnits.Add(new List<Unit>());
        }

        Board.Instance.InitializeBoard();
    }

    private void Start()
    {
        if (debugMode)
        {
            StartingPlayer = debugStartingPlayer;
            FillPlayerUnitList(0, debugPlayer1StartingUnits);
            FillPlayerUnitList(1, debugPlayer2StartingUnits);
            DebugSetupAllUnits();
        }

        Initialize();
    }

    public void Initialize()
    {
        PhaseManager.Instance.levelStartEnter += LevelStartEnter;
        PhaseManager.Instance.levelStartExit += LevelStartExit;
        PhaseManager.Instance.levelEndEnter += LevelEndEnter;
        PhaseManager.Instance.levelEndExit += LevelEndExit;
        PhaseManager.Instance.playerTurnStartEnter += PlayerTurnStartEnter;
        PhaseManager.Instance.playerTurnStartExit += PlayerTurnStartExit;
        PhaseManager.Instance.playerTurnEndEnter += PlayerTurnEndEnter;
        PhaseManager.Instance.playerTurnEndExit += PlayerTurnEndExit;
        PhaseManager.Instance.unitSelectionEnter += UnitSelectionEnter;
        PhaseManager.Instance.unitSelectionExit += UnitSelectionExit;
        PhaseManager.Instance.actionSelectionEnter += ActionSelectionEnter;
        PhaseManager.Instance.actionSelectionExit += ActionSelectionExit;
        PhaseManager.Instance.movementPseudoStateEnter += MovementPseudoStateEnter;
        PhaseManager.Instance.movementPseudoStateExit += MovementPseudoStateExit;
        PhaseManager.Instance.attackSelectionEnter += AttackSelectionEnter;
        PhaseManager.Instance.attackSelectionExit += AttackSelectionExit;
        PhaseManager.Instance.attackTargetSelectionEnter += AttackTargetSelectionEnter;
        PhaseManager.Instance.attackTargetSelectionExit += AttackTargetSelectionExit;
        PhaseManager.Instance.attackPseudoStateEnter += AttackPseudoStateEnter;
        PhaseManager.Instance.attackPseudoStateExit += AttackPseudoStateExit;

        PhaseManager.Instance.Initialize();
    }

    public void StartLevel()
    {
        CurrentPlayerID = StartingPlayer;
    }

    #region State Actions
    private void LevelStartEnter()
    {

    }

    private void LevelStartExit()
    {

    }

    private void LevelEndEnter()
    {

    }

    private void LevelEndExit()
    {

    }

    private void PlayerTurnStartEnter()
    {
        //Anim de debut de tour
        OnPlayerTurnStart?.Invoke();
                                                                                                              
        FreshupUnits(playerUnits[CurrentPlayerID]);
        
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.UnitSelection);
    }

    private void PlayerTurnStartExit()
    {
        CurrentPlayer.EnableInput();
    }

    private void PlayerTurnEndEnter()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.PlayerTurnStart);                                                                      //Change Current Player!
    }

    private void PlayerTurnEndExit()
    {
        CurrentPlayer.DisableInput();
        CurrentPlayerID = (CurrentPlayerID + 1) % players.Length;
    }

    private void UnitSelectionEnter()
    {
        CurrentSelectedUnit = null;
        isMerging = false;
        CurrentPlayer.OnCancel += PlayerEndTurn;                                                    //Debug (Normalement OpenGameplayMenu)
        CurrentPlayer.OnUnitSelection += SelectUnit;
        
        if(playerUnits[CurrentPlayerID].Count == 0)                                                         //Debug
        {
            Debug.LogError("Player " + CurrentPlayerID + " has no unit left!");
            return;
        }

        if (AreAllUnitsUsed())                                       
        {
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.PlayerTurnEnd);
            return;
        }
    }

    private void UnitSelectionExit()
    {
        CurrentPlayer.OnCancel -= PlayerEndTurn;                    //Debug (Normalement OpenGameplayMenu)
        CurrentPlayer.OnUnitSelection -= SelectUnit;
    }

    private void ActionSelectionEnter()
    {
        GetUnitMovementRange();
        if (CurrentPlayer.areRangeDisplayed)
        {
            DisplayUnitMovementRange();                                         //Ajouter if PlayerSettings
            //Display la bonne UI
        }

        CurrentPlayer.OnTileMouseOver += RangeManager.Instance.AddToCurrentPath;
        CurrentPlayer.OnTileSelection += OrderMovement;
        CurrentPlayer.OnAttackButtonPress += EnterAttackTargetSelectionState;
        CurrentPlayer.OnCancel += EnterUnitSelectionState;
    }

    private void ActionSelectionExit()
    {
        CurrentPlayer.OnTileMouseOver -= RangeManager.Instance.AddToCurrentPath;
        CurrentPlayer.OnTileSelection -= OrderMovement;
        CurrentPlayer.OnAttackButtonPress -= EnterAttackTargetSelectionState;
        CurrentPlayer.OnCancel -= EnterUnitSelectionState;

        RangeManager.Instance.ClearTiles();

        if (CurrentPlayer.areRangeDisplayed)
        {
            //Undisplay UI
        }
    }

    private void MovementPseudoStateEnter()
    {
        CurrentSelectedUnit.MoveTo(movementPath);

        if (isMerging)
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.UnitSelection);
        else
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.AttackSelection);

    }

    private void MovementPseudoStateExit()
    {
        tilesInMovementRange = null;
        movementPath = null;
    }

    private void AttackSelectionEnter()
    {
        CurrentPlayer.OnAttackButtonPress += EnterAttackTargetSelectionState;
        CurrentPlayer.OnCancel += EnterUnitSelectionState;
    }

    private void AttackSelectionExit()
    {
        CurrentPlayer.OnAttackButtonPress -= EnterAttackTargetSelectionState;
        CurrentPlayer.OnCancel -= EnterUnitSelectionState;
    }

    private void AttackTargetSelectionEnter()
    {
        GetUnitAttackRange();
        
        if(CurrentPlayer.areRangeDisplayed)
        {
            DisplayUnitAttackRange();
            //Display UI
        }

        CurrentPlayer.OnTileMouseOver += RangeManager.Instance.TargetTile;
        CurrentPlayer.OnTileSelection += OrderAttack;
        CurrentPlayer.OnCancel += EnterAppropriateActionState;
    }

    private void AttackTargetSelectionExit()
    {
        CurrentPlayer.OnTileMouseOver -= RangeManager.Instance.TargetTile;
        RangeManager.Instance.ClearTiles();
        CurrentPlayer.OnTileSelection -= OrderAttack;
        CurrentPlayer.OnCancel -= EnterAppropriateActionState;

        if (CurrentPlayer.areRangeDisplayed)
        {
            //unDisplay UI
        }
    }

    private void AttackPseudoStateEnter()
    {
        CurrentSelectedUnit.Attack(targets);
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.UnitSelection);
    }

    private void AttackPseudoStateExit()
    {
        tilesInAttackRange = null;
        targets = null;
    }
    #endregion

    #region Phase Call and Menu
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
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.PlayerTurnEnd);
    }

    public void EnterUnitSelectionState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.UnitSelection);
    }

    public void EnterActionSelectionState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.ActionSelection);
    }

    public void EnterAttackSelectionState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.AttackSelection);
    }

    public void EnterAttackTargetSelectionState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.AttackTargetSelection);
    }

    public void EnterAppropriateActionState()
    {
        if (CurrentSelectedUnit != null)
        {
            if (CurrentSelectedUnit.CurrentUnitState == UnitState.Fresh)
            {
                PhaseManager.Instance.gameplayState.ChangeState(GameplayState.ActionSelection);
            }
            else if (CurrentSelectedUnit.CurrentUnitState == UnitState.Moved)
            {
                PhaseManager.Instance.gameplayState.ChangeState(GameplayState.AttackSelection);
            }
        }
    }
    #endregion

    #region Inputs
    public void SelectUnit(Unit unit)
    {
        if (IsCurrentPlayerUnit(unit))                                                                                                  
        {
            if (unit.CurrentUnitState == UnitState.Fresh || unit.CurrentUnitState == UnitState.Moved)
            {
                CurrentSelectedUnit = unit;
                EnterAppropriateActionState();
            }
        }
    }

    public void OrderMovement(Tile tile)
    {
        if (tile != null && tilesInMovementRange.Contains(tile))
        {
            if (tile.unit != null)
                isMerging = true;

            movementPath = RangeManager.Instance.GetCurrentPath();

            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.MovementPseudoState);
        }
    }

    private void OrderAttack(Tile tile)
    {
        if (tile != null && tilesInAttackRange.Contains(tile))
        {
            targets = RangeManager.Instance.GetTargets();
            if (CurrentSelectedUnit.UnitAttackPattern.type == AttackPatternType.Slice)
                targets.Add(tile);
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.AttackPseudoState);
        }
    }
    #endregion

    #region Utility
    private void FillPlayerUnitList(int playerID, GameObject[] startingUnits)
    {
        foreach (GameObject unitGameObject in startingUnits)
        {
            Unit unit;
            if ((unit = unitGameObject.GetComponent<Unit>()) != null && playerID < playerUnits.Count)
                playerUnits[playerID].Add(unit);
        }
    }

    public void RemoveUnitFromPlay(Unit unit)
    {
        foreach(List<Unit> unitList in playerUnits)
        {
            if(unitList.Contains(unit))
            {
                unitList.Remove(unit);
                unit.RemoveFromBoard();
                if (unitList.Count <= 0)
                    CheckWinCondition();
                return;                                                                         //Une unit ne peut appartenir qu'a un seul joueur
            }
        }
    }

    public bool IsCurrentPlayerUnit(Unit unit)
    {
        return IsPlayerUnit(CurrentPlayerID, unit);
    }

    public bool IsPlayerUnit(int playerID, Unit unit)
    {
        if (playerID < players.Length)
            return playerUnits[playerID].Contains(unit);
        else
            return false;
    }

    private bool AreAllUnitsUsed()
    {
        foreach (Unit unit in playerUnits[CurrentPlayerID])
        {
            if (unit.CurrentUnitState != UnitState.Used)                                        //Ajouter Stun?
                return false;
        }
        return true;
    }

    private void FreshupUnits(List<Unit> units)
    {
        foreach(Unit unit in units)
        {
            unit.FreshenUp();
        }
    }

    private void GetUnitMovementRange()
    {
        tilesInMovementRange = RangeManager.Instance.GetTilesInMovementRange(CurrentSelectedUnit.CurrentTile);
    }

    private void GetUnitAttackRange()
    {
        tilesInAttackRange = RangeManager.Instance.GetTilesInAttackRange(CurrentSelectedUnit.CurrentTile);
    }

    private void DisplayUnitMovementRange()
    {
        //StartCoroutine(DelayDisplay(RangeManager.Instance.DisplayMovementTiles));
        //StartCoroutine(DelayMovementRangeDisplay());
        RangeManager.Instance.DisplayMovementTiles();
    }

    private void DisplayUnitAttackRange()
    {
        //StartCoroutine(DelayDisplay(RangeManager.Instance.DisplayAttackTiles));
        //StartCoroutine(DelayAttackRangeDisplay());
        RangeManager.Instance.DisplayAttackTiles();
    }

    //private IEnumerator DelayDisplay(Action display)
    //{
    //    yield return new WaitForFixedUpdate();
    //    display?.Invoke();
    //}

    //private IEnumerator DelayMovementRangeDisplay()
    //{
    //    yield return new WaitForFixedUpdate();
    //    RangeManager.Instance.DisplayMovementTiles();
    //}

    //private IEnumerator DelayAttackRangeDisplay()
    //{
    //    yield return new WaitForFixedUpdate();
    //    RangeManager.Instance.DisplayAttackTiles();
    //}

    private void CheckWinCondition()
    {

    }
    #endregion

    #region Debug
    private void DebugSetupAllUnits()
    {
        foreach (List<Unit> unitList in playerUnits)
        {
            foreach(Unit unit in unitList)
            {
                unit.DebugSetUnitPosition();
            }
        }
    }
    #endregion
}


