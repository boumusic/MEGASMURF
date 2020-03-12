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
    public Image unitIconSolo, unitIconDuo, unitIconTrio;
    public Button actionButton;
    public Image actionIcon;

    private SpriteState actionButtonSpriteState;

    private void Start()
    {
        actionButtonSpriteState = new SpriteState();
    }

    public void SelectUnit(Unit unit)
    {
        gameObject.SetActive(true);
        unitName = UIManager.Instance.uIUnitSlotContainer.UnitSlotBehaviourDictionary[unit].shapeName;
        ChangeUnitIcons(unit);                                                     //Solo pour l'instant
        UpdateHealthText(unit.CurrentHitPoint);
        //SetRightButtonAction()
    }

    public void ChangeUnitIcons(Unit unit)
    {
        //Animation
        UpdateUnitIcons(unit);
    }

    public void UpdateUnitIcons(Unit unit)
    {
        unitIconSolo.sprite = unit.selectedUnitIcon;
        actionIcon.sprite = unit.unitActionIcon;
        
        actionButtonSpriteState.pressedSprite = unit.unitActionIconPressed;
        actionButton.spriteState = actionButtonSpriteState;

        if (unit is ShapeUnit)
        {
            ShapeUnit shapeUnit = (ShapeUnit)unit;
            if (shapeUnit.ArmUnit != null)
                unitIconDuo.sprite = shapeUnit.ArmUnit.selectedUnitIcon;
            else
                unitIconDuo.sprite = null;
            if (shapeUnit.LegUnit != null)
                unitIconTrio.sprite = shapeUnit.HeadUnit.selectedUnitIcon;
            else
                unitIconDuo.sprite = shapeUnit.ArmUnit.selectedUnitIcon;
        }
        else
        {
            unitIconDuo.sprite = null;
            unitIconDuo.sprite = null;
        }
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
