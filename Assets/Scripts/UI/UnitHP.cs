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
    public bool isEnemy = false;

    private void Start()
    {
        string name = "";
        if (!isEnemy)
           name  = UnitSettingsManager.Instance.GetName();
        else
        {
            name = GetComponentInParent<Enemy>().gameObject.name;
            name = name.Replace("_", "");
            name = name.Replace("(Clone)", "");
            for (int i = 0; i < 9; i++)
            {
                name = name.Replace(i.ToString(), "");
            }
        }
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
