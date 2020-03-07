using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{

    public static Board Instance
    {
        get
        {
            if (Board.instance == null)
            {
                Board.instance = FindObjectOfType<Board>();
            }
            return Board.instance;
        }
        set
        {
            instance = value;
        }
    }

    public int Columns { get => columns; }
    public int Rows { get => rows; }

    private static Board instance;

    [Range(1, 40f)] public float totalWidth = 20;
    [Range(1, 40f)] public float totalHeight = 20;
    [Range(2, 50)] private int columns = 15;
    [Range(2, 50)] private int rows = 15;
    public float tileDelay = 0.005f;

    [SerializeField] private Room debugRoom;
    //public float tilesOffset;

    public GameObject tilePrefab;

    private Tile[,] tiles;

    private Room currentRoom;

    public void InitializeBoard()
    {
        InitializeBoard(debugRoom);
    }

    public void InitializeBoard(Room room)
    {
        currentRoom = room;
        currentRoom.OrderTiles();
        GenerateTiles();
        StartCoroutine(TileAppear());
        GenerateUnits();
    }

    private void GenerateTiles()
    {
        tiles = new Tile[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                float x = Utility.Interpolate(-totalWidth / 2, totalWidth / 2, 0, columns - 1, i);
                float z = Utility.Interpolate(-totalHeight / 2, totalHeight / 2, 0, rows - 1, j);

                Vector3 position = new Vector3(x, 0f, z);

                LevelElement tile = currentRoom.GetTile(i, j);

                Tile newTile = PoolManager.Instance.GetEntityOfType(tile.GetType()) as Tile;
                //Tile newTile = Instantiate<GameObject>(tilePrefab, position, Quaternion.identity, transform).GetComponent<Tile>();
                if (newTile != null)
                {
                    newTile.gameObject.SetActive(true);
                    newTile.Coords = new Vector2Int(i, j);
                    newTile.transform.position = position;
                    tiles[i, j] = newTile;
                    string name = "Tile (" + i + "," + j + ")";
                    newTile.gameObject.name = name;
                    newTile.transform.localScale = new Vector3(totalWidth / (columns - 1), 1f, totalHeight / (rows - 1));
                }

            }
        }
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                tiles[i, j].CheckNeighbors();
            }
        }
    }

    private IEnumerator TileAppear()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y].Appear();
                yield return new WaitForSeconds(tileDelay);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int j = 0; j < rows; j++)
        {
            float z = Utility.Interpolate(-totalHeight / 2, totalHeight / 2, 0, rows - 1, j);
            float x = totalWidth / 2f;

            Vector3 from = new Vector3(-x, 0f, z);
            Vector3 to = new Vector3(x, 0f, z);
            Gizmos.DrawLine(from, to);
        }

        for (int i = 0; i < columns; i++)
        {
            float x = Utility.Interpolate(-totalWidth / 2, totalWidth / 2, 0, columns - 1, i);
            float z = totalHeight / 2f;

            Vector3 from = new Vector3(x, 0f, z);
            Vector3 to = new Vector3(x, 0f, -z);
            Gizmos.DrawLine(from, to);
        }
    }

    public void GenerateUnits()
    {

    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x > tiles.GetLength(0) - 1 || y > tiles.GetLength(1) - 1)
        {
            return null;
        }
        else
        {
            return tiles[x, y];
        }
    }

    public Tile[,] GetTiles()
    {
        return tiles;
    }

    public Tile GetTile(Vector2 v)
    {
        return GetTile((int)v.x, (int)v.y);
    }

    public List<Tile> GetTiles(List<Vector2> vectors)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (Vector2 v in vectors)
        {
            Tile t = GetTile(v);
            if (t != null)
            {
                tiles.Add(t);
            }
        }
        return tiles;
    }

    public List<Tile> GetTilesBetween(Tile t1, Tile t2, bool diagonales)
    {
        List<Tile> between = new List<Tile>();
        if (t1.Equals(t2) || !t1.IsInLine(t2))
        {
            return between;
        }
        if (t1.Coords.x == t2.Coords.x)
        {
            // Search Down
            if (t1.Coords.y > t2.Coords.y)
            {
                for (float i = t1.Coords.y - 1; i > t2.Coords.y && i >= 0; i--)
                {
                    between.Add(tiles[(int)(t1.Coords.x), (int)i]);
                }
            }
            // Search Up
            else
            {
                for (float i = t1.Coords.y + 1; i < t2.Coords.y && i < rows; i++)
                {
                    between.Add(tiles[(int)(t1.Coords.x), (int)i]);
                }
            }
        }
        else if (t1.Coords.y == t2.Coords.y)
        {
            // Search Left
            if (t1.Coords.x > t2.Coords.x)
            {
                for (float i = t1.Coords.x - 1; i > t2.Coords.x && i >= 0; i--)
                {
                    between.Add(tiles[(int)i, (int)(t1.Coords.y)]);
                }
            }
            // Search Right
            else
            {
                for (float i = t1.Coords.x + 1; i < t2.Coords.x && i < columns; i++)
                {
                    between.Add(tiles[(int)i, (int)(t1.Coords.y)]);
                }
            }
        }
        else if (diagonales)
        {
            if (t1.Coords.y > t2.Coords.y)
            {
                // Search Down Left
                if (t1.Coords.x > t2.Coords.x)
                {
                    for (float i = 1; t1.Coords.x - i > t2.Coords.x && t1.Coords.y - i > t2.Coords.y && t1.Coords.x - i >= 0 && t1.Coords.y - i >= 0; i++)
                    {
                        between.Add(tiles[(int)(t1.Coords.x - i), (int)(t1.Coords.y - i)]);
                    }
                }
                // Search Down Right
                if (t1.Coords.x < t2.Coords.x)
                {
                    for (float i = 1; t1.Coords.x + i < t2.Coords.x && t1.Coords.y - i > t2.Coords.y && t1.Coords.x + i < columns && t1.Coords.y - i >= 0; i++)
                    {
                        between.Add(tiles[(int)(t1.Coords.x + i), (int)(t1.Coords.y - i)]);
                    }
                }
            }
            else
            {
                // Search Up Left
                if (t1.Coords.x > t2.Coords.x)
                {
                    for (float i = 1; t1.Coords.x - i > t2.Coords.x && t1.Coords.y + i < t2.Coords.y && t1.Coords.x - i >= 0 && t1.Coords.y + i < rows; i++)
                    {
                        between.Add(tiles[(int)(t1.Coords.x - i), (int)(t1.Coords.y + i)]);
                    }
                }
                // Search Up Right
                {
                    for (float i = 1; t1.Coords.x + i < t2.Coords.x && t1.Coords.y + i < t2.Coords.y && t1.Coords.x + i < columns && t1.Coords.y + i < rows; i++)
                    {
                        between.Add(tiles[(int)(t1.Coords.x + i), (int)(t1.Coords.y + i)]);
                    }
                }
            }
        }
        return between;
    }
}
