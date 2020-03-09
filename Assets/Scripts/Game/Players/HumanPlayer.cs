using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
    public override void EnableInput()
    {
        InputManager.instance.OnCancel += OnCancel;
        InputManager.instance.OnAttackButtonPress += OnAttackButtonPress;
        InputManager.instance.OnTileMouseOver += OnTileMouseOver;
        InputManager.instance.OnUnitSelection += OnUnitSelection;
        InputManager.instance.OnTileSelection += OnTileSelection;
    }

    public override void DisableInput()
    {
        InputManager.instance.OnCancel -= OnCancel;
        InputManager.instance.OnAttackButtonPress -= OnAttackButtonPress;
        InputManager.instance.OnTileMouseOver -= OnTileMouseOver;
        InputManager.instance.OnUnitSelection -= OnUnitSelection;
        InputManager.instance.OnTileSelection -= OnTileSelection;
    }
}
