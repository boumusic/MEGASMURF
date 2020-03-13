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
    public Image itemIcon, hoveredSlotSprite, selectedSlotSprite;
    public Image unitIconSolo, unitIconDuo_1, unitIconDuo_2, unitIconTrio_1, unitIconTrio_2, unitIconTrio_3;

    private Sprite soloIcon, duoIcon, trioIcon;

    public Unit SlotUnit { get; private set; }
    private Animator anim;
    private string unitName;

    private bool isShapeUnit;
    private int unitMergeLevel;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if(unitNameTMP.text != unitName)
        {
            unitName = unitNameTMP.text;
            SlotUnit.UnitName = unitName;
            UIManager.Instance.UpdateSelectedUnitName(SlotUnit);
        }
    }

    public void Initialize(Unit unit)
    {
        SlotUnit = unit;
        unitName = unit.UnitName;
        unitNameTMP.text = unitName;
        UpdateUnitIcons(unit);
        UpdateDamageText(unit.CurrentHitPoint);
        
        UIManager.Instance.UpdateSelectedUnitName(SlotUnit);

        //ChangeItemIcon(unit.CurrentEquipement.equipementIcon);
    }


    //ICONES ET TEXTE
    public void UpdateUnitIcons(Unit unit)
    {
        soloIcon = unit.selectedUnitIcon;
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

        ResetDisplay();
        DisplayUnitIcons();
    }

    public void DisplayUnitIcons()
    {
        //Animation

        switch (unitMergeLevel)
        {
            case 0:
                unitIconSolo.sprite = soloIcon;
                break;
            case 1:
                unitIconDuo_1.sprite = soloIcon;
                unitIconDuo_2.sprite = duoIcon;
                fusionDoubleAnim();
                break;
            case 2:
                unitIconTrio_1.sprite = soloIcon;
                unitIconTrio_2.sprite = duoIcon;
                unitIconTrio_3.sprite = trioIcon;
                fusionTripleAnim();
                break;
            default:
                break;
        }
    }

    public void ResetDisplay()
    {
        unitIconSolo.gameObject.SetActive(true);
        unitIconDuo_1.gameObject.SetActive(false); 
        unitIconDuo_2.gameObject.SetActive(false); 
        unitIconTrio_1.gameObject.SetActive(false);
        unitIconTrio_2.gameObject.SetActive(false);
        unitIconTrio_3.gameObject.SetActive(false);
    }
    
    public void ChangeItemIcon(Sprite newItemIcon) 
    {
        itemIcon.sprite = newItemIcon;
    }

    public void UpdateHealth(int newHealth)
    {
        if (newHealth < healthValue)
        {
            shapeSlotDamageAnim();
        }
        else if (newHealth > healthValue)
        {
            shapeSlotHealAnim();
        }

        UpdateDamageText(newHealth);
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
        anim.Play("ShapeSlot_Damage");
    }

    public void shapeSlotHealAnim()
    {
        //A call quand l'unité se fait soigner
        anim.Play("ShapeSlot_Heal");
    }

   public void hoverSlotAnim()
    {
        //Quand le joueur a la souris par dessus
        anim.Play("ShapeSlot_Hover");
    }

    public void onSelectedSlotAnim()
    {
        //Quand l'unité est sélectionnée NOTE : Cette anim loop 
        anim.Play("ShapeSlot_Pressed");
    }

    public void fusionDoubleAnim()
    {
        // A jouer quand l'unité passe d'une shape à deux shapes. Le toggle on/off des images se fait 
        //Dans l'anim je disable l'image de l'unité seule

        anim.Play("SoloToDuo");
    }

    public void fusionTripleAnim()
    {
        //Dans l'anim je disable les images des autres unités pour faire apparaître les triples
        anim.Play("DuoToTrio");
    }

    public void DuoMode()
    {
        unitIconSolo.enabled = false;
        unitIconDuo_1.enabled = true;
        unitIconDuo_2.enabled = true;
        unitIconTrio_1.enabled = false;
        unitIconTrio_2.enabled = false;
        unitIconTrio_3.enabled = false;

    }

    public void TrioMode()
    {
        unitIconSolo.enabled = false;
        unitIconDuo_1.enabled = false;
        unitIconDuo_2.enabled = false;
        unitIconTrio_1.enabled = true;
        unitIconTrio_2.enabled = true;
        unitIconTrio_3.enabled = true;
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
