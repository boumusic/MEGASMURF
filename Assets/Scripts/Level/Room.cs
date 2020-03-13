using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelElementRoomSettings
{
    public LevelElement levelElement;
    public Vector3 pos;
    public int orderInTurn = 0;
    public string name = "";
}

[CreateAssetMenu(fileName = "New Room.asset", menuName = "Level/Room", order = 140)]
public class Room : ScriptableObject
{
    public List<LevelElementRoomSettings> tileElements = new List<LevelElementRoomSettings>();
    public List<LevelElementRoomSettings> entityElements = new List<LevelElementRoomSettings>();

    private int selectedTile = 0;
    private int selectedEntity = 0;

    public int SelectedBrush { get => selectedTab == 0 ? selectedTile : selectedEntity; set { if (selectedTab == 0) selectedTile = value; else selectedEntity = value; } }
    public int SelectedTab { get => selectedTab; set => selectedTab = value; }

    public List<LevelElementRoomSettings> CurrentList => selectedTab == 0 ? tileElements : entityElements;

    private int selectedTab = 0;

    private LevelElement[,] orderedTiles;
    private LevelElement[,] orderedEntities;

    public void OrderElements()
    {
        int columns = Board.Instance.Columns;
        int rows = Board.Instance.Rows;

        orderedTiles = new Tile[columns, rows];
        orderedEntities = new LevelElement[columns, rows];

        for (int i = 0; i < tileElements.Count; i++)
        {
            int x = (int)tileElements[i].pos.x;
            int y = (int)tileElements[i].pos.y;
            orderedTiles[x, y] = tileElements[i].levelElement;
            orderedEntities[x, y] = entityElements[i].levelElement;
        }
    }

    public void FixLinks()
    {
        FixArray(tileElements);
        FixArray(entityElements);
    }

    private void FixArray(List<LevelElementRoomSettings> elems)
    {
        for (int i = 0; i < elems.Count; i++)
        {
            if (elems[i].name != "" && elems[i].levelElement == null)
            {
                LevelElement elem = PoolManager.Instance.GetLevelElementOfName(elems[i].name);
                if (elem)
                    elems[i].levelElement = elem;
                else
                {
                    elems[i].levelElement = null;
                    elems[i].name = "";
                }
            }

            if(elems[i].levelElement != null && elems[i].name == "")
            {
                elems[i].name = elems[i].levelElement.name;
            }
        }
    }

    public LevelElement GetTile(int x, int y)
    {
        return orderedTiles[x, y];
    }

    public LevelElement GetEntity(int x, int y)
    {
        return orderedEntities[x, y];
    }
}
