using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private GameObject objectUnderMouse;

    public event Action OnCancel;
    public event Action<Unit> OnUnitSelection;
    public event Action<Tile> OnTileSelection;

    private PointerEventData pointerEventData;

    public Tile currentTile { get; private set; }
    
    private void Awake()
    {
        instance = this;
    }

    public void TileClickCallBack(Tile tile)
    {
        if (tile.unit != null)
            OnUnitSelection?.Invoke(tile.unit);
        else
            OnTileSelection?.Invoke(tile);
    }

    public void SendCancelEvent()
    {
        OnCancel?.Invoke();
    }

    public void SendUnitSelection(Unit unit)
    {
        OnUnitSelection?.Invoke(unit);
    }

    public void SendTileSelection(Tile tile)
    {
        OnTileSelection?.Invoke(tile);
    }

    public void UpdateCurrentTile(Tile tile)
    {
        currentTile = tile;
    }
}
