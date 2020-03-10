using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;
    private List<Brain> aIBrains;
    private int unitIterrator;

    private void Awake()
    {
        instance = this;
    }

    public void StartTurn()
    {
        unitIterrator = 0;
        SequenceManager.Instance.EnQueueAction(ExecuteNextUnitActionsequence, ActionType.AutomaticResume);
    }

    private void ExecuteNextUnitActionsequence()
    {
        if(unitIterrator < BattleManager.Instance.playerUnits[BattleManager.Instance.CurrentPlayerID].Count)
        {
            Enemy currentEnemy;
            if ((currentEnemy = BattleManager.Instance.playerUnits[BattleManager.Instance.CurrentPlayerID][unitIterrator] as Enemy) != null)
                currentEnemy.Sequence();
            SequenceManager.Instance.EnQueueAction(ExecuteNextUnitActionsequence, ActionType.AutomaticResume);
            unitIterrator++;
        }
        else
        {
            SequenceManager.Instance.EnQueueAction(BattleManager.Instance.PlayerEndTurn, ActionType.AutomaticResume);
        }
            
    }

    public void AddBrain(Brain brain)
    {
        aIBrains.Add(brain);
    }

    public void AIDeathCallBack()
    {
        unitIterrator--;
    }
}
