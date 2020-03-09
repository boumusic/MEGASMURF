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
    ActionSelection,
    MovementPseudoState,
    AttackSelection,
    AttackTargetSelection,
    AttackPseudoState
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
    public GameEvent actionSelectionEnter;
    public GameEvent actionSelectionExit;
    public GameEvent movementPseudoStateEnter;
    public GameEvent movementPseudoStateExit;
    public GameEvent attackSelectionEnter;
    public GameEvent attackSelectionExit;
    public GameEvent attackTargetSelectionEnter;
    public GameEvent attackTargetSelectionExit;
    public GameEvent attackPseudoStateEnter;
    public GameEvent attackPseudoStateExit;

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

    private void AttackSelection_Enter()
    {
        PrintDebugMessage("Enter AttackSelection State!");
        attackSelectionEnter?.Invoke();
    }

    private void AttackSelection_Exit()
    {
        PrintDebugMessage("Exit AttackSelection State!");
        attackSelectionExit?.Invoke();
    }

    private void AttackTargetSelection_Enter()
    {
        PrintDebugMessage("Enter AttackTargetSelection State!");
        attackTargetSelectionEnter?.Invoke();
    }

    private void AttackTargetSelection_Exit()
    {
        PrintDebugMessage("Exit AttackTargetSelection State!");
        attackTargetSelectionExit?.Invoke();
    }

    private void AttackPseudoState_Enter()
    {
        PrintDebugMessage("Enter AttackPseudoState State!");
        attackPseudoStateEnter?.Invoke();
    }

    private void AttackPseudoState_Exit()
    {
        PrintDebugMessage("Exit AttackPseudoState State!");
        attackPseudoStateExit?.Invoke();
    }
    #endregion
}
