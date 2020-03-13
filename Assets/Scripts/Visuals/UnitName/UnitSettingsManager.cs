using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSettingsManager : MonoBehaviour
{
    private static UnitSettingsManager instance;
    public static UnitSettingsManager Instance { get { if (!instance) instance = FindObjectOfType<UnitSettingsManager>(); return instance; } }

    public UnitNameSettings nameSettings;
    public UnitGeneralSettings generalSettings;
    private List<string> names = new List<string>();
    public bool allowDuplicates = false;

    private void Awake()
    {
        for (int i = 0; i < nameSettings.names.Count; i++)
        {
            names.Add(nameSettings.names[i]);
        }
    }

    public string GetName()
    {
        int index = Random.Range(0, names.Count-1);

        string name = "";
        if (index >= 0)
        {
            name = names[index];
            if (!allowDuplicates)
            {
                names.RemoveAt(index);
            }
        }
            
        return name;
    }
}
