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
        unitName = UnitSettingsManager.Instance.GetName();
        UpdateText();
    }

    private void UpdateText()
    {
        if(text)
        text.text = unitName;
    }
}
