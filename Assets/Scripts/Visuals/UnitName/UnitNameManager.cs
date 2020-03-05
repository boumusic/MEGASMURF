using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitNameManager : MonoBehaviour
{
    private static UnitNameManager instance;
    public static UnitNameManager Instance { get { if (!instance) instance = FindObjectOfType<UnitNameManager>(); return instance; } }

    public UnitNameSettings settings;
    private List<string> names = new List<string>();
    public bool allowDuplicates = false;

    private void Awake()
    {
        for (int i = 0; i < settings.names.Count; i++)
        {
            names.Add(settings.names[i]);
        }
    }

    public string GetName()
    {
        int index = Random.Range(0, names.Count);
        string name = names[index];
        if(!allowDuplicates)
        {
            names.RemoveAt(index);
        }

        return name;
    }
}
