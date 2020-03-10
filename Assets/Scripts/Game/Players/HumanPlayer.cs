using System;
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
            InputManager.instance.OnCircleButtonPress += CallOnCircleButtonPress;
            InputManager.instance.OnTriangleButtonPress += CallOnTriangleButtonPress;
            InputManager.instance.OnSquareButtonPress += CallOnSquareButtonPress;
            InputManager.instance.OnTileMouseOver += CallOnTileMouseOver;
            InputManager.instance.OnUnitSelection += CallOnUnitSelection;
            InputManager.instance.OnTileSelection += CallOnTileSelection;
        }
    }

    public override void DisableInput()
    {
        if (isEnable)
        {
            isEnable = false;
            InputManager.instance.OnCancel -= CallOnCancel;
            InputManager.instance.OnActionButtonPress -= CallOnActionButtonPress;
            InputManager.instance.OnCircleButtonPress -= CallOnCircleButtonPress;
            InputManager.instance.OnTriangleButtonPress -= CallOnTriangleButtonPress;
            InputManager.instance.OnSquareButtonPress -= CallOnSquareButtonPress;
            InputManager.instance.OnTileMouseOver -= CallOnTileMouseOver;
            InputManager.instance.OnUnitSelection -= CallOnUnitSelection;
            InputManager.instance.OnTileSelection -= CallOnTileSelection;
        }
    }

    //private void OnDestroy()
    //{
    //    if(isEnable)
    //    {
    //        InputManager.instance.OnCancel -= CallOnCancel;
    //        InputManager.instance.OnAttackButtonPress -= CallOnAttackButtonPress;
    //        InputManager.instance.OnTileMouseOver -= CallOnTileMouseOver;
    //        InputManager.instance.OnUnitSelection -= CallOnUnitSelection;
    //        InputManager.instance.OnTileSelection -= CallOnTileSelection;
    //    }
    //}
}
