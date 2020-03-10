using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : ScriptableObject
{
    private static UnitFactory instance;
    public static UnitFactory Instance { get { if (!instance) instance = FindObjectOfType<UnitFactory>(); return instance; } }

    public UnitEncyclopedia unitEncylopedia;
    private Dictionary<BaseUnitType, GameObject> unitDictionary;

    private void Awake()
    {
        unitDictionary = new Dictionary<BaseUnitType, GameObject>();
        InitializeDictionary();
    }


    private void InitializeDictionary()
    {
        foreach(UnitPage unitPage in unitEncylopedia.unitPages)
        {
            if (!unitDictionary.ContainsKey(unitPage.unitType))
                unitDictionary.Add(unitPage.unitType, unitPage.unitPrefab);
            else
                Debug.LogError("UnitEncylopedia: The encylopedia contains 2 units with the same type!");
        }
    }

    public GameObject CreateUnit(BaseUnitType baseUnitType)
    {
        if (unitDictionary.ContainsKey(baseUnitType))
            return Instantiate(unitDictionary[baseUnitType]) as GameObject;
        else
        {
            Debug.LogError("UnitFactory: Trying to create unknown unit");
            return null;
        }
    }
}
