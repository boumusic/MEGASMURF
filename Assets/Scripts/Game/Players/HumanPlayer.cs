﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
    private bool isEnable;

    public override void EnableInput()
    {
        if(!isEnable)
        {
            isEnable = true;
            InputManager.instance.OnCancel += CallOnCancel;
            InputManager.instance.OnActionButtonPress += CallOnActionButtonPress;
            InputManager.instance.OnEndTurnInput += BattleManager.Instance.PlayerEndTurn;
            InputManager.instance.OnCircleButtonPress += CallOnCircleButtonPress;
            InputManager.instance.OnTriangleButtonPress += CallOnTriangleButtonPress;
            InputManager.instance.OnSquareButtonPress += CallOnSquareButtonPress;
            InputManager.instance.OnTileMouseOver += CallOnTileMouseOver;
            InputManager.instance.OnUnitSelection += CallOnUnitSelection;
            InputManager.instance.OnUIUnitSelection += CallOnUIUnitSelection;
            InputManager.instance.OnTileSelection += CallOnTileSelection;
            InputManager.instance.OnOutOfBoardClick += CallOnOutOfBoardClick;

            //Afficher l'UI Player
        }
    }

    public override void DisableInput()
    {
        if (isEnable)
        {
            isEnable = false;
            InputManager.instance.OnCancel -= CallOnCancel;
            InputManager.instance.OnActionButtonPress -= CallOnActionButtonPress;
            InputManager.instance.OnEndTurnInput -= BattleManager.Instance.PlayerEndTurn;
            InputManager.instance.OnCircleButtonPress -= CallOnCircleButtonPress;
            InputManager.instance.OnTriangleButtonPress -= CallOnTriangleButtonPress;
            InputManager.instance.OnSquareButtonPress -= CallOnSquareButtonPress;
            InputManager.instance.OnTileMouseOver -= CallOnTileMouseOver;
            InputManager.instance.OnUnitSelection -= CallOnUnitSelection;
            InputManager.instance.OnUIUnitSelection -= CallOnUIUnitSelection;
            InputManager.instance.OnTileSelection -= CallOnTileSelection;
            InputManager.instance.OnOutOfBoardClick -= CallOnOutOfBoardClick;

            //Desafficher UI Player
        }
    }
}
