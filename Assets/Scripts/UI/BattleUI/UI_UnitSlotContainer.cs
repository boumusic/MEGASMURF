using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitSlotContainer : MonoBehaviour
{
    public Vector2 slotOrigin;
    public float spaceBeetweenSlot;
    public GameObject addUnitButton;
    public Dictionary<Unit, GameObject> UnitSlotDictionary { get; private set; }
    public Dictionary<Unit, UI_ShapeSlotBehavior> UnitSlotBehaviourDictionary { get; private set; }
    private List<Unit> unitList;

    private void Awake()
    {
        addUnitButton.transform.localPosition = slotOrigin;
    }

    public void Add(GameObject unitSlot)
    {
        UI_ShapeSlotBehavior unitSlotBehaviour = unitSlot.GetComponent<UI_ShapeSlotBehavior>();
        if(!unitList.Contains(unitSlotBehaviour.SlotUnit))
        {
            unitList.Add(unitSlotBehaviour.SlotUnit);
            UnitSlotDictionary.Add(unitSlotBehaviour.SlotUnit, unitSlot);

            unitSlot.transform.parent = gameObject.transform;
            unitSlot.transform.localPosition = spaceBeetweenSlot * unitList.Count * Vector3.up;

            //Animation

            addUnitButton.transform.localPosition = spaceBeetweenSlot * (unitList.Count + 1) * Vector3.up;
        }
    }

    public void Remove(Unit unit)
    {
        if(unitList.Contains(unit))
        {
            //RemoveAnimation(SlotDictionary[unit])

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
        //Scroll jusqu'a ce que l'unitslot soit dans la bonne range;
        //HighLight
    }
}
