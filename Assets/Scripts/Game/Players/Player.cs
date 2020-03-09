using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public bool areRangeDisplayed;

    public Action OnCancel;
    public Action OnAttackButtonPress;
    public Action<Tile> OnTileMouseOver;
    public Action<Unit> OnUnitSelection;
    public Action<Tile> OnTileSelection;

    public abstract void EnableInput();

    public abstract void DisableInput();

    public virtual void CallOnCancel()
    {
        OnCancel?.Invoke();
    }

    public virtual void CallOnAttackButtonPress()
    {
        OnAttackButtonPress?.Invoke();
    }

    public virtual void CallOnTileMouseOver(Tile tile)
    {
        OnTileMouseOver?.Invoke(tile);
    }

    public virtual void CallOnUnitSelection(Unit unit)
    {
        OnUnitSelection?.Invoke(unit);
    }

    public virtual void CallOnTileSelection(Tile tile)
    {
        OnTileSelection?.Invoke(tile);
    }
}
