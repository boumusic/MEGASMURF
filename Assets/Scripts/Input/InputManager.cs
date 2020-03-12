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

    public Action OnCancel;
    public Action OnActionButtonPress;
    public Action OnCircleButtonPress;
    public Action OnTriangleButtonPress;
    public Action OnSquareButtonPress;
    public Action OnEndTurnInput;
    public Action<Tile> OnTileMouseOver;
    public Action<Unit> OnUnitSelection;
    public Action<Tile> OnTileSelection;

    private PlayerInput playerInput;

    private InputAction ActionAction;
    private InputAction CancelAction;
    private InputAction CircleAction;
    private InputAction TriangleAction;
    private InputAction SquareAction;

    public Tile currentTile { get; private set; }
    
    private void Awake()
    {
        instance = this;

        playerInput = GetComponent<PlayerInput>();
        ActionAction = playerInput.actions.FindAction("Action");
        CancelAction = playerInput.actions.FindAction("Cancel");
        CircleAction = playerInput.actions.FindAction("Circle");
        TriangleAction = playerInput.actions.FindAction("Triangle");
        SquareAction = playerInput.actions.FindAction("Square");

        ActionAction.performed += SendActionButtonEvent;
        CancelAction.performed += SendCancelEvent;
        CircleAction.performed += SendCircleButtonEvent;
        TriangleAction.performed += SendTriangleButtonEvent;
        SquareAction.performed += SendSquareButtonEvent;
    }

    private void OnDestroy()
    {
        ActionAction.performed -= SendActionButtonEvent;
        CancelAction.performed -= SendCancelEvent;
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

        OnCancel?.Invoke();
    }

    public void SendActionButtonEvent(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        OnActionButtonPress?.Invoke();
    }

    public void SendEndTurnInput()
    {
        OnEndTurnInput?.Invoke();
    }

    public void SendCircleButtonEvent(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        OnCircleButtonPress?.Invoke();
    }
    
    public void SendCircleButtonEvent()
    {
        OnCircleButtonPress?.Invoke();
    }

    public void SendTriangleButtonEvent(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        OnTriangleButtonPress?.Invoke();
    }
    
    public void SendTriangleButtonEvent()
    {
        OnTriangleButtonPress?.Invoke();
    }

    public void SendSquareButtonEvent(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        OnSquareButtonPress?.Invoke();
    }
    
    public void SendSquareButtonEvent()
    {
        OnSquareButtonPress?.Invoke();
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
}
