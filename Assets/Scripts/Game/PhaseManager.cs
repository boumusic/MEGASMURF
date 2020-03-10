using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public enum GameplayState
{
    LevelStart,
    LevelEnd,
    PlayerTurnStart,
    PlayerTurnEnd,
    UnitSelection,
    MovementSelection,
    MovementPseudoState,
    ActionSelection,
    MaestroActionInterSelection,
    ActionTargetSelection,
    ActionPseudoState
}

public class PhaseManager : MonoBehaviour
{
    private static PhaseManager instance;
    public static PhaseManager Instance { get { if (!instance) instance = FindObjectOfType<PhaseManager>(); return instance; } }

    public bool debugPhaseConsol;

    public delegate void GameEvent();
    public GameEvent levelStartEnter;
    public GameEvent levelStartExit;
    public GameEvent levelEndEnter;
    public GameEvent levelEndExit;
    public GameEvent playerTurnStartEnter;
    public GameEvent playerTurnStartExit;
    public GameEvent playerTurnEndEnter;
    public GameEvent playerTurnEndExit;
    public GameEvent unitSelectionEnter;
    public GameEvent unitSelectionExit;
    public GameEvent movementSelectionEnter;
    public GameEvent movementSelectionExit;
    public GameEvent movementPseudoStateEnter;
    public GameEvent movementPseudoStateExit;
    public GameEvent actionSelectionEnter;
    public GameEvent actionSelectionExit;
    public GameEvent maestroActionInterSelectionEnter;
    public GameEvent maestroActionInterSelectionExit;
    public GameEvent actionTargetSelectionEnter;
    public GameEvent actionTargetSelectionExit;
    public GameEvent actionPseudoStateEnter;
    public GameEvent actionPseudoStateExit;

    public StateMachine<GameplayState> gameplayState;

    public void Initialize()
    {
        gameplayState = StateMachine<GameplayState>.Initialize(this);
        gameplayState.ManualUpdate = true;

        gameplayState.ChangeState(GameplayState.PlayerTurnStart);           // Bof... A mettre ailleur (autre appel du battleManager
    }

    private void PrintDebugMessage(string message)
    {
        if (debugPhaseConsol)
            Debug.Log(message);
    }

    #region State
    private void LevelStart_Enter()
    {
        PrintDebugMessage("Enter LevelStart State!");
        levelStartEnter?.Invoke();
    }

    private void LevelStart_Exit()
    {
        PrintDebugMessage("Exit LevelStart State!");
        levelStartExit?.Invoke();
    }

    private void LevelEnd_Enter()
    {
        PrintDebugMessage("Enter LevelEnd State!");
        levelEndEnter?.Invoke();
    }

    private void LevelEnd_Exit()
    {
        PrintDebugMessage("Exit LevelEnd State!");
        levelEndExit?.Invoke();
    }

    private void PlayerTurnStart_Enter()
    {
        PrintDebugMessage("Enter PlayerTurnStart State!");
        playerTurnStartEnter?.Invoke();
    }

    private void PlayerTurnStart_Exit()
    {
        PrintDebugMessage("Exit PlayerTurnStart State!");
        playerTurnStartExit?.Invoke();
    }

    private void PlayerTurnEnd_Enter()
    {
        PrintDebugMessage("Enter PlayerTurnEnd State!");
        playerTurnEndEnter?.Invoke();
    }

    private void PlayerTurnEnd_Exit()
    {
        PrintDebugMessage("Exit PlayerTurnEnd State!");
        playerTurnEndExit?.Invoke();
    }

    private void UnitSelection_Enter()
    {
        PrintDebugMessage("Enter UnitSelection State!");
        unitSelectionEnter?.Invoke();
    }

    private void UnitSelection_Exit()
    {
        PrintDebugMessage("Exit UnitSelection State!");
        unitSelectionExit?.Invoke();
    }

    private void MovementSelection_Enter()
    {
        PrintDebugMessage("Enter MovementSelection State!");
        movementSelectionEnter?.Invoke();
    }

    private void MovementSelection_Exit()
    {
        PrintDebugMessage("Exit MovementSelection State!");
        movementSelectionExit?.Invoke();
    }

    private void MovementPseudoState_Enter()
    {
        PrintDebugMessage("Enter MovementPseudoState State!");
        movementPseudoStateEnter?.Invoke();
    }

    private void MovementPseudoState_Exit()
    {
        PrintDebugMessage("Exit MovementPseudoState State!");
        movementPseudoStateExit?.Invoke();
    }

    private void ActionSelection_Enter()
    {
        PrintDebugMessage("Enter ActionSelection State!");
        actionSelectionEnter?.Invoke();
    }

    private void ActionSelection_Exit()
    {
        PrintDebugMessage("Exit ActionSelection State!");
        actionSelectionExit?.Invoke();
    }

    private void MaestroActionInterSelection_Enter()
    {
        PrintDebugMessage("Enter MaestroActionInterSelection State!");
        maestroActionInterSelectionEnter?.Invoke();
    }

    private void MaestroActionInterSelection_Exit()
    {
        PrintDebugMessage("Exit MaestroActionInterSelection State!");
        maestroActionInterSelectionExit?.Invoke();
    }

    private void ActionTargetSelection_Enter()
    {
        PrintDebugMessage("Enter ActionTargetSelection State!");
        actionTargetSelectionEnter?.Invoke();
    }

    private void ActionTargetSelection_Exit()
    {
        PrintDebugMessage("Exit ActionTargetSelection State!");
        actionTargetSelectionExit?.Invoke();
    }

    private void ActionPseudoState_Enter()
    {
        PrintDebugMessage("Enter ActionPseudoState State!");
        actionPseudoStateEnter?.Invoke();
    }

    private void ActionPseudoState_Exit()
    {
        PrintDebugMessage("Exit ActionPseudoState State!");
        actionPseudoStateExit?.Invoke();
    }
    #endregion
}
