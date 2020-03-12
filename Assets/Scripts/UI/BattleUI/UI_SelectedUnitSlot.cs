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
    public Image actionButtonIcon;

    private bool isShapeUnit;

    private Sprite soloIcon, duoIcon, trioIcon;
    private Sprite actionIcon;
    private Sprite actionIconPressed;
    private Sprite actionIconTouched;
    private Sprite actionCancelIcon;
    private Sprite actionCancelIconPressed;
    private Sprite actionCancelIconTouched;
    

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

        UpdateActionIcons(unit);

        UpdateUnitIcons(unit);

        DisplayUnitIcons();
        DisplayActionIcons();
        UpdateHealthText(unit.CurrentHitPoint);
    }

    public void UpdateActionIcons(Unit unit)
    {
        actionIcon = unit.unitActionIcon;
        actionIconPressed = unit.unitActionIconPressed;
        actionIconTouched = unit.unitActionIconTouched;

        actionCancelIcon = unit.unitActionCancelIcon;
        actionCancelIconPressed = unit.unitActionCancelIconPressed;
        actionCancelIconTouched = unit.unitActionCancelIconTouched;
        DisplayActionIcons();
    }

    public void UpdateUnitIcons(Unit unit)
    {
        if (isShapeUnit = unit is ShapeUnit)
        {
            ShapeUnit shapeUnit = (ShapeUnit)unit;

            soloIcon = shapeUnit.selectedUnitIcon;
            duoIcon = null;
            trioIcon = null;

            if (shapeUnit.ArmUnit != null)
            {
                soloIcon = shapeUnit.shapeLegIcon;
                duoIcon = shapeUnit.ArmUnit.selectedUnitIcon;
            }
            if (shapeUnit.HeadUnit != null)
                trioIcon = shapeUnit.HeadUnit.selectedUnitIcon;
        }
        DisplayUnitIcons();
    }

    public void DisplayUnitIcons()
    {
        //Animation
        unitIconSolo.sprite = soloIcon;

        if (isShapeUnit)
        {
            if (unitIconDuo != null)
            {
                unitIconDuo.sprite = duoIcon;
            }
            if (unitIconTrio != null)
                unitIconTrio.sprite = trioIcon;
        }
    }

    public void DisplayActionIcons()
    {
        actionButtonIcon.sprite = actionIcon;

        actionButtonSpriteState.pressedSprite = actionIconPressed;
        actionButtonSpriteState.highlightedSprite = actionIconTouched;
        actionButton.spriteState = actionButtonSpriteState;
    }

    public void SwitchToCancelButton()
    {
        actionButtonSpriteState.pressedSprite = actionCancelIconPressed;
        actionButtonSpriteState.highlightedSprite = actionCancelIconTouched;
        actionButton.spriteState = actionButtonSpriteState;
    }

    public void SwitchToActionButton()
    {
        actionButtonSpriteState.pressedSprite = actionIconPressed;
        actionButtonSpriteState.highlightedSprite = actionIconTouched;
        actionButton.spriteState = actionButtonSpriteState;
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
