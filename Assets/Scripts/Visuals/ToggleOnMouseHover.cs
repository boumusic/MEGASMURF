using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnMouseHover : MonoBehaviour
{
    public GameObject go;

    private void Start()
    {
        if (go)
            go.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (go)
            go.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (go)
            go.SetActive(false);
    }
}
