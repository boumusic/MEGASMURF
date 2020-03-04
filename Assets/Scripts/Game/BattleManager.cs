using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

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

    private StateMachine<GameState> gameState;

    private void Start()
    {
        gameState = StateMachine<GameState>.Initialize(this);
        gameState.ManualUpdate = true;
        gameState.ChangeState(GameState.PlayerTurn);
    }

    private void PlayerTurn_Enter()
    {
        playerTurnStarted?.Invoke();
    }

    private void EnemyTurn_Enter()
    {
        enemyTurnStarted?.Invoke();
    }

    public void PlayerEndTurn()
    {
        gameState.ChangeState(GameState.EnemyTurn);
    }

    //public void EnterPlayerTurn()
    //{
    //    //Anim de debut de tour
    //    //Entre dans UnitSelection
    //}

    //public void EnterUnitSelection()
    //{
    //    //Abonnement input 
    //      => ClickUnit = SelectUnit()
    //      => cancel = gameplayMenu()
    //    (//Attend un input
    //    //Entre en EnemyUnitDisplay
    //    //Entre en ActionSelection)
    //}

    //public void SelecUnit()
    //{
    //     recup l'unit cliqué
    //     Check enemy ou ally
    //     Allez dans le bon state
    //}
}

public enum GameState
{
    PlayerTurn,
    EnemyTurn,
}
