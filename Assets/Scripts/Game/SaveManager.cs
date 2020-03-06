using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    
    public static void SerializeData()
    {

    }

    public void SaveGame()
    {

        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        // 3
        hits = 0;
        shots = 0;
        shotsText.text = "Shots: " + shots;
        hitsText.text = "Hits: " + hits;

        ClearRobots();
        ClearBullets();
        Debug.Log("Game Saved");
    }


}
