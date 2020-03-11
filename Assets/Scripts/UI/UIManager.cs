using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { if (!instance) instance = FindObjectOfType<UIManager>(); return instance; } }

    public UI_ShapemudFill uIShapemud;
    public UI_ShapeSlotContainer uIUnitSlotContainer;
    public UI_ShapeSlotSelected SelectedUnitSlot;

    public void UpdateShapeMud(int newValue)
    {
        uIShapemud.UpdateShapemudText(newValue);
    }

    public void UpdateUnitHealth(Unit unit, int newHealth)
    {
        uIUnitSlotContainer.UpdateUnitHealth(unit, newHealth);
    }

    public void SelectUnit(Unit unit)
    {
        uIUnitSlotContainer.SelectUnit(unit);
        SelectedUnitSlot.SelectUnit(unit);
    }

    public void AddNewUnitUISlot(Unit unit)
    {
        GameObject unitSlot = PoolManager.Instance.GetEntityOfType(UI_ShapeSlotBehavior);

        unitSlot.GetComponent<UI_ShapeSlotBehavior>().Initialize(unit);

        uIUnitSlotContainer.Add(unitSlot);
    }

    public void RemoveUnitUISlot(Unit unit)
    {
        uIUnitSlotContainer.Remove(unit);
    }
}
