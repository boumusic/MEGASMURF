using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextRoomButton : MonoBehaviour
{

    public Button button;
    public Image image;

    private void Update()
    {
        if(Board.Instance.maestro != null && Board.Instance.maestro.SpawnID >= 0 && BattleManager.Instance.CurrentPlayerID == 0)
        {
            image.enabled = true;
            button.enabled = true;
        }
        else
        {
            image.enabled = false;
            button.enabled = false;
        }
    }

    public void NextRoom()
    {
        if (Board.Instance.maestro != null && Board.Instance.maestro.SpawnID >= 0)
        {
            Board.Instance.NextRoom();
        }
    }

}
