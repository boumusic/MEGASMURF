using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputManager instance;

    public LayerMask MouseInteractable;

    private GameObject objectUnderMouse;

    private void Awake()
    {
        instance = this;
  
    }

    private void FixedUpdate()
    {
        
    }
}
