using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveManager: MonoBehaviour
{

    public static SaveManager Instance;

    public UnitFactory unitFactory;

    public void Awake()
    {
        if(SaveManager.Instance == null)
        {
            SaveManager.Instance = this;
        }
    }

    public void SaveGame()
    {

        Save save = new Save();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");

    }

    public void LoadGame()
    {

        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            GameManager.ShapeMud = save.shapemud;
            GameManager.SkillTree.tree = save.skilltree;
            GameManager.units = save.team.GetUnits();

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

}
