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
    public Maestro MaestroUnit { get; private set; }

    private int StartingPlayer { get; set; }
    public Player CurrentPlayer => (CurrentPlayerID < players.Length) ? players[CurrentPlayerID] : null;
    public int CurrentPlayerID { get; private set; }

    public event Action OnPlayerTurnStart;
    public Action OnBattleModeButtonPress;


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
        GameManager.units = new List<Unit>();
    }

    private void Start()
    {

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
        for (int i = 0; i < players.Length; i++)
        {
            playerUnits.Add(new List<Unit>());
        }

        if (Board.Instance.dungeon.Count > 0)
        {
            Board.Instance.InitializeBoard(Board.Instance.dungeon[0]);
        }
        else
        {
            Board.Instance.InitializeBoard();
        }
        if (debugMode)
        {
            StartingPlayer = debugStartingPlayer;
            FillPlayerUnitList(0, debugPlayer1StartingUnits);
            FillPlayerUnitList(1, debugPlayer2StartingUnits);
            DebugSetupAllUnits();
        }

        Initialize();
        CurrentPlayerID = StartingPlayer;
    }

    public void LightStart()
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
        SequenceManager.Instance.EnQueueAction(OnPlayerTurnStart, ActionType.AutomaticResume);

        SequenceManager.Instance.EnQueueAction(CheckForInfiniteRange, ActionType.AutomaticResume);

        SequenceManager.Instance.EnQueueAction(FreshenUpCurrentPlayerUnits, ActionType.AutomaticResume);

        SequenceManager.Instance.EnQueueAction(EnterUnitSelectionState, ActionType.AutomaticResume);

        
    }

    private void PlayerTurnStartExit()
    {
        
    }

    private void PlayerTurnEndEnter()
    {
        SequenceManager.Instance.EnQueueAction(EnterPlayerTurnStartState, ActionType.AutomaticResume); 
    }

    private void PlayerTurnEndExit()
    {
        SequenceManager.Instance.EnQueueAction(CurrentPlayer.DisableInput, ActionType.AutomaticResume);
        SequenceManager.Instance.EnQueueAction(ChangeCurrentPlayer, ActionType.AutomaticResume);
    }

    private void UnitSelectionEnter()
    {
        SequenceManager.Instance.EnQueueAction(UnitSelectionVariableInitialization, ActionType.AutomaticResume);
        SequenceManager.Instance.EnQueueAction(UnitSelectionActivateInputs, ActionType.AutomaticResume); 
        SequenceManager.Instance.EnQueueAction(CheckAutomaticTurnEnd, ActionType.AutomaticResume);
        SequenceManager.Instance.EnQueueAction(CurrentPlayer.EnableInput, ActionType.AutomaticResume);
    }

    private void UnitSelectionExit()
    {
        SequenceManager.Instance.EnQueueAction(UnitSelectionDeactivateInputs, ActionType.AutomaticResume);
    }

    private void UnitSelectionVariableInitialization()
    {
        UIManager.Instance.UnselectUnit(CurrentSelectedUnit);
        CurrentSelectedUnit = null;
        isMerging = false;
    }

    private void UnitSelectionActivateInputs()
    {
        CurrentPlayer.OnCancel += PlayerEndTurn;                                                    //Debug (Normalement OpenGameplayMenu)
        CurrentPlayer.OnUnitSelection += SelectUnit;
        UIManager.Instance.EnableEndTurnButton();
        UIManager.Instance.EnableNextLevelButton();
    }

    private void UnitSelectionDeactivateInputs()
    {
        CurrentPlayer.OnCancel -= PlayerEndTurn;                    //Debug (Normalement OpenGameplayMenu)
        CurrentPlayer.OnUnitSelection -= SelectUnit;
        UIManager.Instance.DesableEndTurnButton();
        UIManager.Instance.DesableNextLevelButton();
    }

    private void MovementSelectionEnter()
    {
        SequenceManager.Instance.EnQueueAction(MovementSelectionDisplayInformation, ActionType.AutomaticResume);
        SequenceManager.Instance.EnQueueAction(MovementSelectionActivateInputs, ActionType.AutomaticResume);
    }

    private void MovementSelectionExit()
    {
        SequenceManager.Instance.EnQueueAction(MovementSelectionUndisplayInformation, ActionType.AutomaticResume);
        SequenceManager.Instance.EnQueueAction(MovementSelectionDeactivateInputs, ActionType.AutomaticResume);
    }

    private void MovementSelectionDisplayInformation()
    {
        GetUnitMovementRange();
        if (CurrentPlayer.areRangeDisplayed)
        {
            DisplayUnitMovementRange();                                         //Ajouter if PlayerSettings
            //Display la bonne UI
        }
    }

    private void MovementSelectionUndisplayInformation()
    {
        RangeManager.Instance.ClearTiles();

        if (CurrentPlayer.areRangeDisplayed)
        {
            //Undisplay UI
        }
    }

    private void MovementSelectionActivateInputs()
    {
        CurrentPlayer.OnTileMouseOver += RangeManager.Instance.AddToCurrentPath;
        CurrentPlayer.OnTileSelection += OrderMovement;
        CurrentPlayer.OnActionButtonPress += EnterRightActionTargetSelectionState;
        OnBattleModeButtonPress += EnterRightActionTargetSelectionState;
        CurrentPlayer.OnCancel += EnterUnitSelectionState;
    }

    private void MovementSelectionDeactivateInputs()
    {
        CurrentPlayer.OnTileMouseOver -= RangeManager.Instance.AddToCurrentPath;
        CurrentPlayer.OnTileSelection -= OrderMovement;
        CurrentPlayer.OnActionButtonPress -= EnterRightActionTargetSelectionState;
        OnBattleModeButtonPress -= EnterRightActionTargetSelectionState;
        CurrentPlayer.OnCancel -= EnterUnitSelectionState;
    }

    private void MovementPseudoStateEnter()
    {
        SequenceManager.Instance.EnQueueAction(MovementPseudoStateMove, ActionType.ManualResume);
        SequenceManager.Instance.EnQueueAction(MovementPseudoStateChangeState, ActionType.AutomaticResume);
    }

    private void MovementPseudoStateExit()
    {
        SequenceManager.Instance.EnQueueAction(MovementPseudoStateEraseData, ActionType.AutomaticResume);
    }

    private void MovementPseudoStateMove()
    {
        CurrentSelectedUnit.MoveTo(movementPath, SequenceManager.Instance.Resume);
    }

    private void MovementPseudoStateChangeState()
    {
        if (isMerging)
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.UnitSelection);
        else
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.ActionSelection);
    }

    private void MovementPseudoStateEraseData()
    {
        tilesInMovementRange = null;
        movementPath = null;
    }

    private void ActionSelectionEnter()
    {
        SequenceManager.Instance.EnQueueAction(ActionSelectionActivateInput, ActionType.AutomaticResume);
        
    }

    private void ActionSelectionExit()
    {
        SequenceManager.Instance.EnQueueAction(ActionSelectionDeactivateInput, ActionType.AutomaticResume);
    }

    private void ActionSelectionActivateInput()
    {
        CurrentPlayer.OnActionButtonPress += EnterRightActionTargetSelectionState;
        OnBattleModeButtonPress += EnterRightActionTargetSelectionState;
        CurrentPlayer.OnCancel += EnterUnitSelectionState;
    }

    private void ActionSelectionDeactivateInput()
    {
        CurrentPlayer.OnActionButtonPress -= EnterRightActionTargetSelectionState;
        OnBattleModeButtonPress -= EnterRightActionTargetSelectionState;
        CurrentPlayer.OnCancel -= EnterUnitSelectionState;
    }

    private void MaestroActionInterSelectionEnter()
    {
        SequenceManager.Instance.EnQueueAction(MaestroActionInterSelectionActivateInput, ActionType.AutomaticResume);
    }
    
    private void MaestroActionInterSelectionExit()
    {
        SequenceManager.Instance.EnQueueAction(MaestroActionInterSelectionDeactivateInput, ActionType.AutomaticResume);
    }

    private void MaestroActionInterSelectionActivateInput()
    {
        //Activate ShapeSelectionUI
        CurrentPlayer.OnCircleButtonPress += SelectCircleShape;
        CurrentPlayer.OnTriangleButtonPress += SelectTriangleShape;
        CurrentPlayer.OnSquareButtonPress += SelectSquareShape;
        OnBattleModeButtonPress += EnterAppropriateActionState;
        CurrentPlayer.OnCancel += EnterAppropriateActionState;
        UIManager.Instance.EnableShapeSelectionUI();
    }

    private void MaestroActionInterSelectionDeactivateInput()
    {
        //deactivate ShapeSelectionUI
        CurrentPlayer.OnCircleButtonPress -= SelectCircleShape;
        CurrentPlayer.OnTriangleButtonPress -= SelectTriangleShape;
        CurrentPlayer.OnSquareButtonPress -= SelectSquareShape;
        OnBattleModeButtonPress -= EnterAppropriateActionState;
        CurrentPlayer.OnCancel -= EnterAppropriateActionState;
        UIManager.Instance.DesableShapeSelectionUI();
    }

    private void ActionTargetSelectionEnter()                                                                                       // BIG CHANGES
    {
        SequenceManager.Instance.EnQueueAction(ActionTargetSelectionGetAndDisplayInformation, ActionType.AutomaticResume);
        SequenceManager.Instance.EnQueueAction(ActionTargetSelectionActivateInput, ActionType.AutomaticResume);
    }

    private void ActionTargetSelectionExit()
    {
        SequenceManager.Instance.EnQueueAction(ActionTargetSelectionDeactivateInput, ActionType.AutomaticResume);
        SequenceManager.Instance.EnQueueAction(ActionTargetSelectionUndisplayInformation, ActionType.AutomaticResume);
    }

    private void ActionTargetSelectionGetAndDisplayInformation()
    {
        GetUnitActionRange();

        if (CurrentPlayer.areRangeDisplayed)
        {
            DisplayUnitActionRange();
            //Display UI
        }
    }

    private void ActionTargetSelectionUndisplayInformation()
    {
        RangeManager.Instance.ClearTiles();

        if (CurrentPlayer.areRangeDisplayed)
        {
            //unDisplay UI
        }
    }

    private void ActionTargetSelectionActivateInput()
    {
        CurrentPlayer.OnTileMouseOver += RangeManager.Instance.TargetTile;
        CurrentPlayer.OnTileSelection += OrderAction;
        OnBattleModeButtonPress += CancelToRightActionSelectionState;
        CurrentPlayer.OnCancel += CancelToRightActionSelectionState;
    }

    private void ActionTargetSelectionDeactivateInput()
    {
        CurrentPlayer.OnTileMouseOver -= RangeManager.Instance.TargetTile;
        CurrentPlayer.OnTileSelection -= OrderAction;
        OnBattleModeButtonPress -= CancelToRightActionSelectionState;
        CurrentPlayer.OnCancel -= CancelToRightActionSelectionState;
    }

    private void ActionPseudoStateEnter()
    {
        SequenceManager.Instance.EnQueueAction(ActionPseudoStateAction, ActionType.ManualResume);
        SequenceManager.Instance.EnQueueAction(ActionPseudoStateChangeState, ActionType.AutomaticResume);
    }

    private void ActionPseudoStateExit()
    {
        SequenceManager.Instance.EnQueueAction(ActionPseudoStateEraseData, ActionType.AutomaticResume);
    }

    private void ActionPseudoStateAction()
    {
        CurrentSelectedUnit.Action(targets, SequenceManager.Instance.Resume);
    }

    private void ActionPseudoStateChangeState()
    {
        EnterAppropriateActionState();
    }

    private void ActionPseudoStateEraseData()
    {
        tilesInActionRange = null;
        targets = null;
        SelectedUnitTypeToBeSummon = BaseUnitType.NONE;
    }
    #endregion

    #region Phase Call and Menu

    public void ResetState()
    {
        PhaseManager.Instance.ResetState();
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

    public void EnterPlayerTurnStartState()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.PlayerTurnStart);
    }

    public void PlayerEndTurn()
    {
        PhaseManager.Instance.gameplayState.ChangeState(GameplayState.PlayerTurnEnd);
        //Clean Sequencer?
    }
    
    public void PlayerEndTurnDelay()
    {
        SequenceManager.Instance.EnQueueAction(PlayerEndTurn, ActionType.AutomaticResume);
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
        //Maestro
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

    public void EnterRightActionTargetSelectionState()
    {
        if (CurrentSelectedUnit is Maestro)
        {
            EnterMaestroActionInterSelectionState();
        }
        else
            EnterActionTargetSelectionState();
    }

    private void CancelToRightActionSelectionState()
    {
        if (CurrentSelectedUnit is Maestro)
            EnterMaestroActionInterSelectionState();
        else
            EnterAppropriateActionState();
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
                UIManager.Instance.SelectUnit(unit);
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

    private void SelectCircleShape()
    {
        if(UnitFactory.Instance.UnitDictionary[BaseUnitType.Circle].unitCost > GameManager.ShapeMud)
        {
            //UISendNotEnoughShapemudFeedBack();
        }
        else
        {
            SelectedUnitTypeToBeSummon = BaseUnitType.Circle;
            PayShapeMudCost(UnitFactory.Instance.UnitDictionary[BaseUnitType.Circle].unitCost);
            EnterActionTargetSelectionState();
        }
    }

    private void SelectTriangleShape()
    {
        if (UnitFactory.Instance.UnitDictionary[BaseUnitType.Triangle].unitCost > GameManager.ShapeMud)
        {
            //UISendNotEnoughShapemudFeedBack();
        }
        else
        {
            SelectedUnitTypeToBeSummon = BaseUnitType.Triangle;
            PayShapeMudCost(UnitFactory.Instance.UnitDictionary[BaseUnitType.Triangle].unitCost);
            EnterActionTargetSelectionState();
        }
    }

    private void SelectSquareShape()
    {
        if (UnitFactory.Instance.UnitDictionary[BaseUnitType.Square].unitCost > GameManager.ShapeMud)
        {
            //UISendNotEnoughShapemudFeedBack();
        }
        else
        {
            SelectedUnitTypeToBeSummon = BaseUnitType.Square;
            PayShapeMudCost(UnitFactory.Instance.UnitDictionary[BaseUnitType.Square].unitCost);
            EnterActionTargetSelectionState();
        }
    }

    public void CallOnBattleModeButtonPress()
    {
        OnBattleModeButtonPress?.Invoke();
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
        {
            playerUnits[playerID].Add(unit);
            if (playerID == 0)
            {
                UIManager.Instance.AddNewUnitUISlot(unit);
                if (unit is Maestro)
                    MaestroUnit = (Maestro)unit;
            }
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
                UIManager.Instance.RemoveUnitUISlot(unit);
                return;                                                                         //Une unit ne peut appartenir qu'a un seul joueur
            }
        }
        CheckForInfiniteRange();
    }

    private void ChangeCurrentPlayer()
    {
        CurrentPlayerID = (CurrentPlayerID + 1) % players.Length;
    }

    private void CheckForInfiniteRange()
    {
        if(playerUnits != null & playerUnits.Count > 1 && playerUnits[1].Count == 0 && !Board.Instance.SpawnersActive())
        {
            foreach(Unit u in playerUnits[0])
            {
                u.HasInfiniteMoveRange = true;
            }
        }
        else
        {
            foreach (Unit u in playerUnits[0])
            {
                u.HasInfiniteMoveRange = false;
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

    private void CheckAutomaticTurnEnd()
    {
        if (playerUnits[CurrentPlayerID].Count == 0)                                                        
        {
            Debug.Log("Player " + CurrentPlayerID + " has no unit left!");
            return;                                                                                                 //A enlever
        }

        if (AreAllUnitsUsed())
        {
            PhaseManager.Instance.gameplayState.ChangeState(GameplayState.PlayerTurnEnd);
            return;
        }
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

    private void FreshenUpCurrentPlayerUnits()
    {
        FreshupUnits(playerUnits[CurrentPlayerID]);
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

    private void GetUnitActionRange()
    {
        tilesInActionRange = RangeManager.Instance.GetTilesInAttackRange(CurrentSelectedUnit.CurrentTile);
    }

    private void DisplayUnitMovementRange()
    {
        StartCoroutine(DelayDisplay(RangeManager.Instance.DisplayMovementTiles));
    }

    private void DisplayUnitActionRange()
    {
        StartCoroutine(DelayDisplay(RangeManager.Instance.DisplayAttackTiles));
    }

    private IEnumerator DelayDisplay(Action display)
    {
        yield return new WaitForFixedUpdate();
        display?.Invoke();
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

    #region Don t belong here
    public void PayShapeMudCost(int cost, Action action = null)
    {
        //Animation
        action?.Invoke();
        GameManager.ShapeMud -= cost;
    }
    #endregion
}