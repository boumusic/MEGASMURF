using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveManager: MonoBehaviour
{

    public static SaveManager Instance;

    public bool resetSave;
    public int debugMud;

    //public UnitFactory unitFactory;

    public void Awake()
    {
        if(SaveManager.Instance == null)
        {
            SaveManager.Instance = this;
        }
    }

    private void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {

        Save save = new Save();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameSave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");

    }

    public void LoadGame()
    {
        GameManager.SkillTree = new SkillTree();
        for (int i = 0; i < 24; i++)
        {
            GameManager.SkillTree.tree.Add(i, false);
        }
        if (File.Exists(Application.persistentDataPath + "/Gamesave.save"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameSave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            if (debugMud > 0)
            {
                GameManager.ShapeMud = debugMud;
            }
            else
            {
                GameManager.ShapeMud = save.shapemud;
            }
            if (resetSave || save.skilltree == null || save.skilltree.Count == 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    GameManager.SkillTree.tree[i] = false;
                }
            }
            else
            {
                GameManager.SkillTree = new SkillTree();
                GameManager.SkillTree.tree = save.skilltree;
            }
            //GameManager.units = save.team.GetUnits();

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }
        if(GameManager.ShapeMud == 0)
        {
            GameManager.ShapeMud = 10;
        }
        BattleManager.Instance.StartLevel();
    }

}
