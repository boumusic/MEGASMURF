using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { if (!instance) instance = FindObjectOfType<UIManager>(); return instance; } }

    public UI_ShapemudFill uIShapemud;
    public UI_UnitSlotContainer uIUnitSlotContainer;
    public UI_SelectedUnitSlot SelectedUnitSlot;
    public UI_ShapeSlotBehavior UnitSlotBehaviourPrefab;

    public GameObject EndTurnButton;
    public GameObject NextLevelButton;
    public GameObject ShapeSelectionUI;

    private void Awake()
    {
        SelectedUnitSlot.UnselectUnit();
        DesableEndTurnButton();
        DesableNextLevelButton();
        DesableShapeSelectionUI();
    }

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

    public void UnselectUnit(Unit unit)
    {
        uIUnitSlotContainer.UnselectUnit(unit);
        //SelectedUnitSlot.UnselectUnit();
    }

    public void EnableEndTurnButton()
    {
        EndTurnButton.SetActive(true);
    }

    public void DesableEndTurnButton()
    {
        EndTurnButton.SetActive(false);
    }

    public void EnableNextLevelButton()
    {
        NextLevelButton.SetActive(true);
    }

    public void DesableNextLevelButton()
    {
        NextLevelButton.SetActive(false);
    }

    public void EnableShapeSelectionUI()
    {
        ShapeSelectionUI.SetActive(true);
    }

    public void DesableShapeSelectionUI()
    {
        ShapeSelectionUI.SetActive(false);
    }

    public void AddNewUnitUISlot(Unit unit)
    {
        GameObject unitSlot = PoolManager.Instance.GetEntityOfType(UnitSlotBehaviourPrefab.GetType()).gameObject;

        unitSlot.GetComponent<UI_ShapeSlotBehavior>().Initialize(unit);

        uIUnitSlotContainer.Add(unitSlot);
    }

    public void RemoveUnitUISlot(Unit unit)
    {
        uIUnitSlotContainer.Remove(unit);
    }

    public void UpdateSelectedUnitName(Unit unit)
    {
        SelectedUnitSlot.UpdateName(unit);
    }

    public void CheckSelectedUnitRemoved(Unit unit)
    {
        if (unit == SelectedUnitSlot.SelectedUnit)
        {
            if (BattleManager.Instance.MaestroUnit != null)
                SelectUnit(BattleManager.Instance.MaestroUnit);
            else
                SelectUnit(uIUnitSlotContainer.GetNextUnit(uIUnitSlotContainer.unitList.IndexOf(unit)));
        }
    }

    public void SwitchToActionButton()
    {
        SelectedUnitSlot.SwitchToActionButton();
    }

    public void SwitchToCancelButton()
    {
        SelectedUnitSlot.SwitchToCancelButton();
    }

    public void UpdateUnitIcon(Unit unit)
    {
        SelectedUnitSlot.UpdateUnitIcons(unit);
    }

    //DeactivationUI?
}
