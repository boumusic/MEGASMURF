using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
    public override void EnableInput()
    {
        InputManager.instance.OnCancel += CallOnCancel;
        InputManager.instance.OnAttackButtonPress += CallOnAttackButtonPress;
        InputManager.instance.OnTileMouseOver += CallOnTileMouseOver;
        InputManager.instance.OnUnitSelection += CallOnUnitSelection;
        InputManager.instance.OnTileSelection += CallOnTileSelection;
    }

    public override void DisableInput()
    {
        InputManager.instance.OnCancel -= CallOnCancel;
        InputManager.instance.OnAttackButtonPress -= CallOnAttackButtonPress;
        InputManager.instance.OnTileMouseOver -= CallOnTileMouseOver;
        InputManager.instance.OnUnitSelection -= CallOnUnitSelection;
        InputManager.instance.OnTileSelection -= CallOnTileSelection;
    }
}
