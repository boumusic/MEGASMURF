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
        for (unitIterrator = 0; unitIterrator < BattleManager.Instance.playerUnits[BattleManager.Instance.CurrentPlayerID].Count; unitIterrator++)
        {
            Enemy currentEnemy;
            if ((currentEnemy = BattleManager.Instance.playerUnits[BattleManager.Instance.CurrentPlayerID][unitIterrator] as Enemy) != null)
                currentEnemy.Sequence();
        }

        BattleManager.Instance.PlayerEndTurn();
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
