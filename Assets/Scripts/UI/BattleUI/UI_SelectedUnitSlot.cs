using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_SelectedUnitSlot : UIElement
{
    private int healthValue;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitHealthText;
    public Image unitIconSolo, unitIconDuo, UniIconTrio;
    public Toggle battleModeToggle;

    public void SelectUnit(Unit unit)
    {
        gameObject.SetActive(true);
        unitName = UIManager.Instance.uIUnitSlotContainer.UnitSlotBehaviourDictionary[unit].shapeName;
        unitIconSolo.sprite = unit.unitIcon;                                                                //Solo pour l'instant
        UpdateHealthText(unit.CurrentHitPoint);
        SetBattleToStandBy();
        //SetRightButtonAction()
    }

    public void UnselectUnit()
    {
        gameObject.SetActive(false);
    }

    public void UpdateHealthText(int newHealthValue)
    {
        healthValue = newHealthValue;

        unitHealthText.text = healthValue.ToString();
    }

    public void SetBattleToAction()
    {
        battleModeToggle.isOn = true;
    }

    public void SetBattleToStandBy()
    {
        battleModeToggle.isOn = false;
    }
}
