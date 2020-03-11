using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ShapeSlotBehavior : UIElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Displayed Stuff")]
    public int healthValue;
    public TextMeshProUGUI shapeName;

    //[Header("Masked Stuff")]
    public TextMeshProUGUI healthText;
    public Image itemIcon, shapeIcon, hoveredSlotSprite, selectedSlotSprite;

    public Unit SlotUnit { get; private set; }
    private Animator ShapeSlot_anim;
    

    private void Awake()
    {

    }

    public void Initialize(Unit unit)
    {
        SlotUnit = unit;
        UpdateDamageText(unit.CurrentHitPoint);
        ChangeshapeIcon(unit.unitIcon);
        //ChangeItemIcon(unit.CurrentEquipement.equipementIcon);
    }

    public void UpdateHealth(int newHealth)
    {
        if(newHealth < healthValue)
        {
            shapeSlotDamageAnim();
        }
        else if(newHealth > healthValue)
        {
            shapeSlotHealAnim();
        }

        UpdateDamageText(newHealth);
    }

    //ICONES ET TEXTE
    public void ChangeItemIcon(Sprite newItemIcon) 
    {
        itemIcon.sprite = newItemIcon;
    }

    public void ChangeshapeIcon(Sprite newShapeIcon)
    {
        shapeIcon.sprite = newShapeIcon;
    }

    public void UpdateDamageText(int newHealthValue)
    {
        healthValue = newHealthValue;
        
        healthText.text = healthValue.ToString();
    }


    //ANIMATIONS

    public void shapeSlotDamageAnim()
    {

    }

    public void shapeSlotHealAnim()
    {
        
    }


    //SELECTION

    public void selectSlot()
    {
        Debug.Log("Personnage selectionné -- Penser à changer le sprite et faire apparaitre le perso en bas à droite");
        selectedSlotSprite.enabled = true;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredSlotSprite.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredSlotSprite.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
