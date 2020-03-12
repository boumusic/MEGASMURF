using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitSlotContainer : UIElement
{
    public Vector2 slotOrigin;
    public float spaceBeetweenSlot;
    public GameObject scrollViewContent;
    //public GameObject addUnitButton;

    public List<Unit> unitList;
    public Dictionary<Unit, GameObject> UnitSlotDictionary { get; private set; }
    public Dictionary<Unit, UI_ShapeSlotBehavior> UnitSlotBehaviourDictionary { get; private set; }
    

    private void Awake()
    {
        //addUnitButton.transform.localPosition = slotOrigin;

        unitList = new List<Unit>();
        UnitSlotDictionary = new Dictionary<Unit, GameObject>();
        UnitSlotBehaviourDictionary = new Dictionary<Unit, UI_ShapeSlotBehavior>();
    }

    public void Add(GameObject unitSlot)
    {
        UI_ShapeSlotBehavior unitSlotBehaviour = unitSlot.GetComponent<UI_ShapeSlotBehavior>();
        if(!unitList.Contains(unitSlotBehaviour.SlotUnit))
        {
            unitList.Add(unitSlotBehaviour.SlotUnit);
            UnitSlotDictionary.Add(unitSlotBehaviour.SlotUnit, unitSlot);
            UnitSlotBehaviourDictionary.Add(unitSlotBehaviour.SlotUnit, unitSlotBehaviour);

            unitSlot.transform.parent = scrollViewContent.transform;
            unitSlot.SetActive(true);
            //Animation
        }
    }

    public void Remove(Unit unit)
    {
        if(unitList.Contains(unit))
        {
            //RemoveAnimation(SlotDictionary[unit])
            UnitSlotDictionary[unit].SetActive(false);
            UIManager.Instance.CheckSelectedUnitRemoved(unit);

            unitList.Remove(unit);
            UnitSlotDictionary.Remove(unit);
            UnitSlotBehaviourDictionary.Remove(unit);
        }
    }

    public void UpdateUnitHealth(Unit unit, int newHealth)
    {
        if(unitList.Contains(unit))
            UnitSlotBehaviourDictionary[unit].UpdateHealth(newHealth);
    }

    public void SelectUnit(Unit unit)
    {
        if (unitList.Contains(unit))
        {
            //Scroll jusqu'a ce que l'unitslot soit dans la bonne range;
            UnitSlotBehaviourDictionary[unit].SelectUnit();
        }
    }

    public void UnselectUnit(Unit unit)
    {
        if (unitList.Contains(unit))
            UnitSlotBehaviourDictionary[unit].UnselectUnit();
    }

    public Unit GetNextUnit(int index)
    {
        return unitList[(index + 1) % unitList.Count];
    }
}
