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

    public Maestro MaestroUnit { get; private set; }
    public List<ShapeUnit> ShapeUnits { get; private set; }
    public List<Enemy> Enemies { get; private set; }

    private void Start()
    {
        gameState = StateMachine<GameState>.Initialize(this);
        gameState.ManualUpdate = true;
        gameState.ChangeState(GameState.PlayerTurn);
    }

    public void PlayerEndTurn()
    {
        gameState.ChangeState(GameState.EnemyTurn);
    }

    private void PlayerTurn_Enter()
    {
        //Anim de debut de tour
        playerTurnStarted?.Invoke();
        //UnExaustedPlayerUnit()
        //Entre dans UnitSelection
    }

    private void EnemyTurn_Enter()
    {
        //Animation
        enemyTurnStarted?.Invoke();
        //UnExaustedEnemyUnit()
        //EnterStartPlayerTurn()
    }

    private void UnitSelection_Enter()
    {
        //Check Si allUnitExausted() -> si oui Enter EnemyTurn
        //Abonnement input 
        //=> OnClick = OnclickActionSwitch(entity)  (appel la methode SelectUnit si cela correspond à une unit)
        //=> OnCancel = gameplayMenu()
    }

    private void UnitSelection_Exit()
    {
        //Desabonne tous les events
        //=> OnClick = null
        //=> OnCancel = null
    }

    

    private void ActionSelection_Enter()
    {
        //DisplayRangeDeplacement
        //Display la bonne UI
        //=> OnClick = OnClickActionSwitch() (active moveTo? EnterAttackSele)
        //=> OnCancel = EnterUnitSelection()
    }

    private void ActionSelection_Exit()
    {
        //Undisplay UI
        //=> OnCancel = null
    }

    private void AttackSelection_Enter()
    {
        //Display attack Range
        //=> OnClick = OnClickActionSwitch() (check si tile -> check si tileIsInRange -> Unit.Attack)
        //if (Unit.hasMoved)
        //=> OnCancel = EnterUnitSelection()
        //else 
        //=> OnCancel = EnterActionSelection()
    }

    private void AttackSelection_Exit()
    {
        //unDisplay UI
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
}


