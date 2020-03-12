using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ShapeSlotBehavior : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Displayed Stuff")]
    public int healthValue;

    public TextMeshProUGUI unitNameTMP;
    public TextMeshProUGUI healthText;
    public Image itemIcon, shapeIcon, shapeIconMiddle, shapeIconTop, hoveredSlotSprite, selectedSlotSprite;

    public Unit SlotUnit { get; private set; }
    private Animator ShapeSlot_anim;
    private string unitName;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if(unitNameTMP.text != unitName)
        {
            unitName = unitNameTMP.text;
            SlotUnit.name = unitName;
            UIManager.Instance.UpdateSelectedUnitName(SlotUnit);
        }
    }

    public void Initialize(Unit unit)
    {
        SlotUnit = unit;
        unitName = unit.name;
        unitNameTMP.text = unit.name;
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
        //A call quand l'unité se fait taper.
    }

    public void shapeSlotHealAnim()
    {
        //A call quand l'unité se fait soigner
    }

   public void hoverSlotAnim()
    {
        //Quand le joueur a la souris par dessus
    }

    public void onSelectedSlotAnim()
    {
        //Quand l'unité est sélectionnée NOTE : Cette anim loop 
    }

    public void fusionDoubleAnim()
    {
        // A jouer quand l'unité passe d'une shape à deux shapes. Le toggle on/off des images se fait 
        //Dans l'anim je disable l'image de l'unité seule
    }

    public void fusionTripleAnim()
    {
        //Dans l'anim je disable les images des autres unités pour faire apparaître les triples
    }

    //SELECTION

    public void OnSelectSlotButtonPress()
    {
        Debug.Log("Personnage selectionné -- Penser à changer le sprite et faire apparaitre le perso en bas à droite");
        InputManager.instance.SendUnitSelection(SlotUnit);
    }

    public void SelectUnit()
    {
        selectedSlotSprite.enabled = true;
    }

    public void UnselectUnit()
    {
        selectedSlotSprite.enabled = false;

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
