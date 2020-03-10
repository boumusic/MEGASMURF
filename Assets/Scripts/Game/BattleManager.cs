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
    public BaseUnitType SelectedUnitTypeToBeSummon { get; private set; }
    private List<Tile> tilesInMovementRange;
    private Stack<Tile> movementPath;
    private List<Tile> tilesInActionRange;
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
        PhaseManager.Instance.movementSelectionEnter += MovementSelectionEnter;
        PhaseManager.Instance.movementSelectionExit += MovementSelectionExit;
        PhaseManager.Instance.movementPseudoStateEnter += MovementPseudoStateEnter;
        PhaseManager.Instance.movementPseudoStateExit += MovementPseudoStateExit;
        PhaseManager.Instance.actionSelectionEnter += ActionSelectionEnter;
        PhaseManager.Instance.actionSelectionExit += ActionSelectionExit;
        PhaseManager.Instance.maestroActionInterSelectionEnter += MaestroActionInterSelectionEnter;
        PhaseManager.Instance.maestroActionInterSelectionExit += MaestroActionInterSelectionExit;
        PhaseManager.Instance.actionTargetSelectionEnter += ActionTargetSelectionEnter;
        PhaseManager.Instance.actionTargetSelectionExit += ActionTargetSelectionExit;
        PhaseManager.Instance.actionPseudoStateEnter += ActionPseudoStateEnter;
        PhaseManager.Instance.actionPseudoStateExit += ActionPseudoStateExit;

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

        CurrentPlayer.EnableInput();
    }

    private void UnitSelectionExit()
    {
        CurrentPlayer.OnCancel -= PlayerEndTurn;                    //Debug (Normalement OpenGameplayMenu)
        CurrentPlayer.OnUnitSelection -= SelectUnit;
    }

    private void MovementSelectionEnter()
    {
        GetUnitMovementRange();
        if (CurrentPlayer.areRangeDisplayed)
        {
            DisplayUnitMovementRange();                                         //Ajouter if PlayerSettings
            //Display la bonne UI
        }

        CurrentPlayer.OnTileMouseOver += RangeManager.Instance.AddToCurrentPath;
        CurrentPlayer.OnTileSelection += OrderMovement;
        CurrentPlayer.OnActionButtonPress += EnterActionTargetSelectionState;
        CurrentPlayer.OnCancel += EnterUnitSelectionState;
    }

    private void MovementSelectionExit()
    {
        CurrentPlayer.OnTileMouseOver -= RangeManager.Instance.AddToCurrentPath;
        CurrentPlayer.OnTileSelection -= OrderMovement;
        CurrentPlayer.OnActionButtonPress -= EnterActionTargetSelectionState;
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
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.ActionSelection);

    }

    private void MovementPseudoStateExit()
    {
        tilesInMovementRange = null;
        movementPath = null;
    }

    private void ActionSelectionEnter()
    {
        CurrentPlayer.OnActionButtonPress += EnterActionTargetSelectionState;
        CurrentPlayer.OnCancel += EnterUnitSelectionState;
    }

    private void ActionSelectionExit()
    {
        CurrentPlayer.OnActionButtonPress -= EnterActionTargetSelectionState;
        CurrentPlayer.OnCancel -= EnterUnitSelectionState;
    }

    private void MaestroActionInterSelectionEnter()
    {
        CurrentPlayer.OnCircleButtonPress += SelectCircleShape;
        CurrentPlayer.OnTriangleButtonPress += SelectTriangleShape;
        CurrentPlayer.OnSquareButtonPress += SelectSquareShape;
    }
    
    private void MaestroActionInterSelectionExit()
    {
        CurrentPlayer.OnCircleButtonPress -= SelectCircleShape;
        CurrentPlayer.OnTriangleButtonPress -= SelectTriangleShape;
        CurrentPlayer.OnSquareButtonPress -= SelectSquareShape;
    }

    private void ActionTargetSelectionEnter()                                                                                       // BIG CHANGES
    {
        GetUnitAttackRange();
        
        if(CurrentPlayer.areRangeDisplayed)
        {
            DisplayUnitActionRange();
            //Display UI
        }

        CurrentPlayer.OnTileMouseOver += RangeManager.Instance.TargetTile;
        CurrentPlayer.OnTileSelection += OrderAction;
        CurrentPlayer.OnCancel += EnterAppropriateActionState;                                                           
    }

    private void ActionTargetSelectionExit()
    {
        CurrentPlayer.OnTileMouseOver -= RangeManager.Instance.TargetTile;
        RangeManager.Instance.ClearTiles();
        CurrentPlayer.OnTileSelection -= OrderAction;
        CurrentPlayer.OnCancel -= EnterAppropriateActionState;

        if (CurrentPlayer.areRangeDisplayed)
        {
            //unDisplay UI
        }
    }

    private void ActionPseudoStateEnter()
    {
        CurrentSelectedUnit.Action(targets);
        EnterAppropriateActionState();
    }

    private void ActionPseudoStateExit()
    {
        tilesInActionRange = null;
        targets = null;
        SelectedUnitTypeToBeSummon = BaseUnitType.NONE;
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

    public void EnterMovementSelectionState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.MovementSelection);
    }

    public void EnterActionSelectionState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.ActionSelection);
    }

    public void EnterMaestroActionInterSelectionState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.MaestroActionInterSelection);
    }

    public void EnterActionTargetSelectionState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.ActionTargetSelection);
    }

    public void EnterAppropriateActionState()
    {
        if (CurrentSelectedUnit != null)
        {
            if (CurrentSelectedUnit.CurrentUnitState == UnitState.Fresh)
            {
                PhaseManager.Instance.gameplayState.ChangeState(GameplayState.MovementSelection);
            }
            else if (CurrentSelectedUnit.CurrentUnitState == UnitState.Moved)
            {
                PhaseManager.Instance.gameplayState.ChangeState(GameplayState.ActionSelection);
            }
            else if(CurrentSelectedUnit.CurrentUnitState == UnitState.Used)
            {
                PhaseManager.Instance.gameplayState.ChangeState(GameplayState.UnitSelection);
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

    public void SelectAction()
    {
        if (CurrentSelectedUnit is Maestro)
        {
            EnterMaestroActionInterSelectionState();
        }
        else
            EnterActionTargetSelectionState();
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

    private void OrderAction(Tile tile)
    {
        if (tile != null && tilesInActionRange.Contains(tile))
        {
            targets = RangeManager.Instance.GetTargets();
            if (CurrentSelectedUnit.UnitAttackPattern.type == AttackPatternType.Slice)
                targets.Add(tile);
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.ActionPseudoState);
        }
    }
    #endregion

    #region Utility
    private void FillPlayerUnitList(int playerID, GameObject[] startingUnits)
    {
        foreach (GameObject unitGameObject in startingUnits)
        {
            AddUnitToPlayerUnitList(playerID, unitGameObject);
        }
    }

    public void AddUnitToPlayerUnitList(int playerID, GameObject unitGameObject)
    {
        Unit unit;
        if ((unit = unitGameObject.GetComponent<Unit>()) != null && playerID < players.Length)
            playerUnits[playerID].Add(unit);
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

    private void SelectCircleShape()
    {
        SelectedUnitTypeToBeSummon = BaseUnitType.Circle;
    }

    private void SelectTriangleShape()
    {
        SelectedUnitTypeToBeSummon = BaseUnitType.Triangle;
    }

    private void SelectSquareShape()
    {
        SelectedUnitTypeToBeSummon = BaseUnitType.Square;
    }

    private void GetUnitMovementRange()
    {
        tilesInMovementRange = RangeManager.Instance.GetTilesInMovementRange(CurrentSelectedUnit.CurrentTile);
    }

    private void GetUnitAttackRange()
    {
        tilesInActionRange = RangeManager.Instance.GetTilesInAttackRange(CurrentSelectedUnit.CurrentTile);
    }

    private void DisplayUnitMovementRange()
    {
        //StartCoroutine(DelayDisplay(RangeManager.Instance.DisplayMovementTiles));
        //StartCoroutine(DelayMovementRangeDisplay());
        RangeManager.Instance.DisplayMovementTiles();
    }

    private void DisplayUnitActionRange()
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


