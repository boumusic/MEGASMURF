using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public enum GameState
{
    PlayerTurn,
    EnemyTurn,
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

    //public void EnterStartPlayerTurn()
    //{
    //    //Anim de debut de tour
    //    //Entre dans UnitSelection
    //}

    public void ExitStartPlayerTurn()
    {
        //rien
    }

    public void EnterUnitSelection()
    {
        //Check Si allUnitExausted() -> si oui Enter EnemyTurn
        //Abonnement input 
        //=> OnClick = OnclickActionSwitch(entity)  (appel la methode SelectUnit si cela correspond à une unit)
        //=> OnCancel = gameplayMenu()
    }

    public void ExitUnitSelection()
    {
        //Desabonne tous les events
        //=> OnClick = null
        //=> OnCancel = null
    }

    private void SelectUnit(Unit unit)
    {
         //Check si type d'entity = Unit
         // Si oui : check enemy, Shape ou maestro
         //          Check si l'unit est !exausted
         //          Entrer dans le bon state
         // Si non : rien faire
         // Allez dans le bon state (check si unit has Moved)
    }

    public void EnterActionSelection()
    {
        //DisplayRangeDeplacement
        //Display la bonne UI
        //=> OnClick = OnClickActionSwitch() (active moveTo? EnterAttackSele)
        //=> OnCancel = EnterUnitSelection()
    }

    public void ExitActionSelection()
    {
        //Undisplay UI
        //=> OnCancel = null
    }

    public void EnterAttackSelection()
    {
        //Display attack Range
        //=> OnClick = OnClickActionSwitch() (check si tile -> check si tileIsInRange -> Unit.Attack)
        //if (Unit.hasMoved)
        //=> OnCancel = EnterUnitSelection()
        //else 
        //=> OnCancel = EnterActionSelection()
    }

    public void ExitAttackSelection()
    {
        //unDisplay UI
        //=> OnClick = null
        //=> OnCancel = null
    }
   

    public void EnterStartEnemyTurn()
    {
        //Animation
        //EnterStartPlayerTurn()
        //
    }
}


