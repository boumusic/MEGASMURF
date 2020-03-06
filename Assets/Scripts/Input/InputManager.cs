using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private GameObject objectUnderMouse;

    public event Action OnCancel;
    public event Action OnAttackButtonPress;
    public event Action<Tile> OnTileMouseOver;
    public event Action<Unit> OnUnitSelection;
    public event Action<Tile> OnTileSelection;

    private PlayerInput playerInput;

    private InputAction AttackAction;
    private InputAction CancelAction;

    public Tile currentTile { get; private set; }
    
    private void Awake()
    {
        instance = this;

        playerInput = GetComponent<PlayerInput>();
        AttackAction = playerInput.actions.FindAction("Attack");
        CancelAction = playerInput.actions.FindAction("Cancel");

        AttackAction.performed += SendAttackButtonEvent;
        CancelAction.performed += SendCancelEvent;
    }

    public void TileClickCallBack(Tile tile)
    {
        if (tile.unit != null)
            OnUnitSelection?.Invoke(tile.unit);
        OnTileSelection?.Invoke(tile);
    }

    public void SendCancelEvent(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        Debug.Log("Cancel button!");
        OnCancel?.Invoke();
    }

    public void SendAttackButtonEvent(InputAction.CallbackContext context)
    {

        if (context.phase != InputActionPhase.Performed)
            return;

        Debug.Log("Attack button!");
        OnAttackButtonPress?.Invoke();
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
        OnTileMouseOver?.Invoke(tile);
    }

    private void CreateGameplayActions()
    {
        var AttackAction = new InputAction("AttackAction");
    }
}
