using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitHP : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI orderText;

    private void Start()
    {
        string name = UnitSettingsManager.Instance.GetName();
        UpdateName(name);
    }

    public void UpdateOrder(int id)
    {
        orderText.text = id.ToString();
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
