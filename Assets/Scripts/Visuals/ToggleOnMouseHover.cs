using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnMouseHover : MonoBehaviour
{
    public GameObject go;

    private void Start()
    {
        go.SetActive(false);
    }

    private void OnMouseEnter()
    {
        go.SetActive(true);
    }

    private void OnMouseExit()
    {
        go.SetActive(false);
    }
}
