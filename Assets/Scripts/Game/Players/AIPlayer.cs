using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    public override void EnableInput()
    {
        InputManager.instance.OnCancel += BattleManager.Instance.OpenGameplayMenu;
        AIManager.instance.StartTurn();
    }

    public override void DisableInput()
    {
        InputManager.instance.OnCancel -= BattleManager.Instance.OpenGameplayMenu;
        //Stop L'IAManager
    }
}
