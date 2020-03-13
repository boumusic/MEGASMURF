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
    public Image unitIconSolo, unitIconDuo_1, unitIconDuo_2, unitIconTrio_1, unitIconTrio_2, unitIconTrio_3;
    public Button actionButton;
    public Image actionButtonIcon;
    //public MouseOverButton mouseOverScript;

    

    private Sprite soloIcon, duoIcon, trioIcon;
    private Sprite actionIcon;
    private Sprite actionIconPressed;
    //private Sprite actionIconTouched;
    private Sprite actionCancelIcon;
    private Sprite actionCancelIconPressed;
    //private Sprite actionCancelIconTouched;

    private bool isShapeUnit;
    private int unitMergeLevel;

    private Animator anim;

    public Unit SelectedUnit { get; private set; }

    private SpriteState actionButtonSpriteState;

    private void Start()
    {
        actionButtonSpriteState = new SpriteState();
        unitMergeLevel = 0;
        anim = gameObject.GetComponent<Animator>();
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

        SelectedSwitchAnim();
    }

    public void UpdateActionIcons(Unit unit)
    {
        actionIcon = unit.unitActionIcon;
        actionIconPressed = unit.unitActionIconPressed;
        //actionIconTouched = unit.unitActionIconTouched;

        actionCancelIcon = unit.unitActionCancelIcon;
        actionCancelIconPressed = unit.unitActionCancelIconPressed;
        //actionCancelIconTouched = unit.unitActionCancelIconTouched;
        DisplayActionIcons();
    }

    public void UpdateUnitIcons(Unit unit)
    {
        soloIcon = unit.selectedUnitIcon;
        duoIcon = null;
        trioIcon = null;
        unitMergeLevel = 0;

        if (isShapeUnit = unit is ShapeUnit)
        {
            ShapeUnit shapeUnit = (ShapeUnit)unit;

            if (shapeUnit.ArmUnit != null)
            {
                soloIcon = shapeUnit.shapeLegIcon;
                duoIcon = shapeUnit.ArmUnit.selectedUnitIcon;
                unitMergeLevel = 1;
            }
            if (shapeUnit.HeadUnit != null)
            {
                trioIcon = shapeUnit.HeadUnit.selectedUnitIcon;
                unitMergeLevel = 2;
            }
        }

        DisplayUnitIcons();
    }

    public void DisplayUnitIcons()
    {
        //Animation
        ResetSprite();

        switch (unitMergeLevel)
        {
            case 0:
                unitIconSolo.sprite = soloIcon;
                break;
            case 1:
                unitIconDuo_1.sprite = soloIcon;
                unitIconDuo_2.sprite = duoIcon;
                
                break;
            case 2:
                unitIconTrio_1.sprite = soloIcon;
                unitIconTrio_2.sprite = duoIcon;
                unitIconTrio_3.sprite = trioIcon;
                break;
            default:
                break;
        }

        ActivateUnitIcons(unitMergeLevel);
    }

    public void DisplayActionIcons()
    {
        actionButtonIcon.sprite = actionIcon;

        actionButtonSpriteState.pressedSprite = actionIconPressed;
        //actionButtonSpriteState.highlightedSprite = actionIconTouched;
        actionButton.spriteState = actionButtonSpriteState;

        //mouseOverScript.baseSprite = actionIcon;
        //mouseOverScript.mouseOverSprite = actionIconTouched;
    }

    public void ActivateUnitIcons(int unitDisplayLevel)
    {
        switch (unitDisplayLevel)
        {
            case -1:
                unitIconSolo.gameObject.SetActive(false);

                unitIconDuo_1.gameObject.SetActive(false);
                unitIconDuo_2.gameObject.SetActive(false);

                unitIconTrio_1.gameObject.SetActive(false);
                unitIconTrio_2.gameObject.SetActive(false);
                unitIconTrio_3.gameObject.SetActive(false);
                break;
            case 0:
                unitIconSolo.gameObject.SetActive(true);

                unitIconDuo_1.gameObject.SetActive(false);
                unitIconDuo_2.gameObject.SetActive(false);

                unitIconTrio_1.gameObject.SetActive(false);
                unitIconTrio_2.gameObject.SetActive(false);
                unitIconTrio_3.gameObject.SetActive(false);
                break;
            case 1:
                unitIconSolo.gameObject.SetActive(false);

                unitIconDuo_1.gameObject.SetActive(true);
                unitIconDuo_2.gameObject.SetActive(true);

                unitIconTrio_1.gameObject.SetActive(false);
                unitIconTrio_2.gameObject.SetActive(false);
                unitIconTrio_3.gameObject.SetActive(false);
                break;
            case 2:
                unitIconSolo.gameObject.SetActive(false);

                unitIconDuo_1.gameObject.SetActive(false);
                unitIconDuo_2.gameObject.SetActive(false);

                unitIconTrio_1.gameObject.SetActive(true);
                unitIconTrio_2.gameObject.SetActive(true);
                unitIconTrio_3.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SwitchToCancelButton()
    {
        actionButtonIcon.sprite = actionCancelIcon;

        actionButtonSpriteState.pressedSprite = actionCancelIconPressed;
        //actionButtonSpriteState.highlightedSprite = actionCancelIconTouched;
        actionButton.spriteState = actionButtonSpriteState;

        //mouseOverScript.baseSprite = actionCancelIcon;
        //mouseOverScript.mouseOverSprite = actionCancelIconTouched;
        //mouseOverScript.UpdateSprites();
    }

    public void SwitchToActionButton()
    {
        actionButtonIcon.sprite = actionIcon;

        actionButtonSpriteState.pressedSprite = actionIconPressed;
        //actionButtonSpriteState.highlightedSprite = actionIconTouched;
        actionButton.spriteState = actionButtonSpriteState;

        
        //mouseOverScript.baseSprite = actionIcon;
        //mouseOverScript.mouseOverSprite = actionIconTouched;
        //mouseOverScript.UpdateSprites();
    }

    public void UnselectUnit()
    {
        gameObject.SetActive(false);
    }

    public void UpdateHealthText(int newHealthValue)
    {
        if(newHealthValue < healthValue)
            SelectedDamageAnim();

        healthValue = newHealthValue;

        unitHealthText.text = healthValue.ToString();
    }

    private void ResetSprite()
    {
        unitIconSolo.sprite = null;
        
        unitIconDuo_1.sprite = null;
        unitIconDuo_2.sprite = null;
        
        unitIconTrio_1.sprite = null;
        unitIconTrio_2.sprite = null;
        unitIconTrio_3.sprite = null;
    }

    public void UpdateName(Unit unit)
    {
        unitName.text = unit.name;
    }

    //ANIMATIONS
    private void Update()
    {
       
    }
    public void SelectedDamageAnim()
    {
        //Quand le joueur prend des dégâts
        anim.Play("SelectedDamage");
    }

    public void SelectedSwitchAnim()
    {
        //Quand le joueur change d'unités
        anim.Play("SelectedSwitch");
    }

    public void SelectedBattleModeOnAnim()
    {
        //Mode attaque activé
    }

    public void SelectedBattleModeOffAnim()
    {
        //Mode attaque desactivé
    }

    public void SelectedSoloToDuo()
    {
        anim.Play("SelectedSoloToDuo");

    }
    public void SelectedDuoToTrio()
    {
        anim.Play("SelectedDuoToTrio");
    }

    public void SelectedInvokeShapesOn()
    {
        anim.Play("SelectedInvokeShapesOn");
    }

    public void SelectedInvokeShapesOff()
    {
        anim.Play("SelectedInvokeShapesOff");
    }

    //public void UpdateName(Unit unit)
    //{
    //    unitName.text = unit.name;
    //}
}
