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
    public Image actionIcon;

    public void SelectUnit(Unit unit)
    {
        gameObject.SetActive(true);
        unitName = UIManager.Instance.uIUnitSlotContainer.UnitSlotBehaviourDictionary[unit].shapeName;
        unitIconSolo.sprite = unit.selectedUnitIcon;                                                                //Solo pour l'instant
        UpdateHealthText(unit.CurrentHitPoint);
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



    //ANIMATIONS

    public void SelectedDamageAnim()
    {
        //Quand le joueur prend des dégâts
    }

    public void SelectedSwitchAnim()
    {
        //Quand le joueur change d'unités
    }

    public void SelectedBattleModeOnAnim()
    {
        //Mode attaque activé
    }

    public void SelectedBattleModeOffAnim()
    {
        //Mode attaque desactivé
    }

    

}
