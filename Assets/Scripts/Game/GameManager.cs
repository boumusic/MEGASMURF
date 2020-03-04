using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { if (!instance) instance = FindObjectOfType<GameManager>(); return instance; } }

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
}

public enum GameState
{
    PlayerTurn,
    EnemyTurn,
}
