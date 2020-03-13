using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitHP : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform parent;

    private void Start()
    {
        if(parent) transform.parent = parent;
        string name = UnitSettingsManager.Instance.GetName();
        UpdateName(name);
    }

    public void UpdateName(string name)
    {
        nameText.text = name;
    }

    public void UpdateHealth(int amount)
    {
        healthText.text = amount.ToString();
    }
}
