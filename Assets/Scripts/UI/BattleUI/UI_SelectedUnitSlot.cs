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

    public Unit SelectedUnit { get; private set; }

    private SpriteState actionButtonSpriteState;

    private void Start()
    {
        actionButtonSpriteState = new SpriteState();
    }

    public void SelectUnit(Unit unit)
    {
        gameObject.SetActive(true);
        unitName = UIManager.Instance.uIUnitSlotContainer.UnitSlotBehaviourDictionary[unit].unitNameTMP;
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
        actionIcon.sprite = unit.unitActionIcon;
        
        actionButtonSpriteState.pressedSprite = unit.unitActionIconPressed;
        actionButtonSpriteState.highlightedSprite = unit.unitActionIconTouched;
        actionButton.spriteState = actionButtonSpriteState;

        if (unit is ShapeUnit)
        {
            ShapeUnit shapeUnit = (ShapeUnit)unit;

            unitIconSolo.sprite = shapeUnit.selectedUnitIcon;
            unitIconDuo.sprite = null;
            unitIconTrio.sprite = null;

            if (shapeUnit.ArmUnit != null)
            {
                unitIconSolo.sprite = shapeUnit.shapeLegIcon;
                unitIconDuo.sprite = shapeUnit.ArmUnit.selectedUnitIcon;
            }  
            if (shapeUnit.HeadUnit != null)
                unitIconTrio.sprite = shapeUnit.HeadUnit.selectedUnitIcon;
        }
        else
        {
            unitIconSolo.sprite = unit.selectedUnitIcon;
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

    public void UpdateName(Unit unit)
    {
        unitName.text = unit.name;
    }
}
