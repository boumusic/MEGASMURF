using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    private static UnitFactory instance;
    public static UnitFactory Instance { get { if (!instance) instance = FindObjectOfType<UnitFactory>(); return instance; } }

    public UnitEncyclopedia unitEncylopedia;
    public Dictionary<BaseUnitType, UnitBlueprint> UnitDictionary { get; private set; }

    private void Awake()
    {
        UnitDictionary = new Dictionary<BaseUnitType, UnitBlueprint>();
        InitializeDictionary();
    }


    private void InitializeDictionary()
    {
        foreach(UnitBlueprint unitBlueprint in unitEncylopedia.unitBlueprints)
        {
            BaseUnitType currentUnitType = unitBlueprint.unitPrefab.GetComponent<Unit>().unitBase.unitType;
            if (!UnitDictionary.ContainsKey(currentUnitType))
                UnitDictionary.Add(currentUnitType, unitBlueprint);
            else
                Debug.LogError("UnitEncylopedia: The encylopedia contains 2 units with the same type!");
        }
    }

    public GameObject CreateUnit(BaseUnitType baseUnitType)
    {
        if (UnitDictionary.ContainsKey(baseUnitType))
            return Instantiate(UnitDictionary[baseUnitType].unitPrefab) as GameObject;
        else
        {
            Debug.LogError("UnitFactory: Trying to create unknown unit");
            return null;
        }
    }
}
