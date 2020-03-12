using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    public override void EnableInput()
    {
        InputManager.instance.OnCancel += BattleManager.Instance.OpenGameplayMenu;
        SequenceManager.Instance.EnQueueAction(Board.Instance.NewSpawnersTurn, ActionType.AutomaticResume);
        SequenceManager.Instance.EnQueueAction(AIManager.instance.StartTurn, ActionType.AutomaticResume);
    }

    public override void DisableInput()
    {
        InputManager.instance.OnCancel -= BattleManager.Instance.OpenGameplayMenu;
        //Stop L'IAManager
    }
}
