using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : UIButton
{
    public override void OnClick()
    {
        base.OnClick();
        BattleManager.Instance.PlayerEndTurn();
    }
}
