using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ShapeSlotBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Displayed Stuff")]
    public int healthValue;
    public TextMeshProUGUI shapeName;

    [Header("Masked Stuff")]
    private TextMeshProUGUI healthText;
    private Image itemIcon, shapeIcon, hoveredSlotSprite, selectedSlotSprite;
    private Animator ShapeSlot_anim;

    void Start()
    {
      updateDamageText(healthValue);
        selectedSlotSprite = gameObject.transform.GetChild(0).GetComponent<Image>();
        hoveredSlotSprite = gameObject.transform.GetChild(1).GetComponent<Image>();
    }

    //ICONES ET TEXTE
    public void changeItemIcon(Sprite newItemIcon) 
    {
        itemIcon = gameObject.transform.Find("ParentPanel/ItemIcon").GetComponent<Image>();
        itemIcon.sprite = newItemIcon;
    
    }

    public void changeshapeIcon(Sprite newShapeIcon)
    {
        shapeIcon = gameObject.transform.Find("ParentPanel/ShapeIcon_Background/ShapeIcon").GetComponent<Image>();
        shapeIcon.sprite = newShapeIcon;
    }

    public void updateDamageText(int newHealthValue)
    {
        healthValue = newHealthValue;
        healthText = gameObject.transform.Find("ParentPanel/HealthIcon/HealthNumberText").GetComponent<TextMeshProUGUI>();
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
}
