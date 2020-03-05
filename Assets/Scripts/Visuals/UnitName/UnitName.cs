using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitName : MonoBehaviour
{
    private string unitName;
    [SerializeField] private TextMeshPro text;

    private void Start()
    {
        unitName = UnitNameManager.Instance.GetName();
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = unitName;
    }
}
