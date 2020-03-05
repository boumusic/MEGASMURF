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

    private static Board instance;

    [Range(1, 40f)] public float totalWidth = 20;
    [Range(1, 40f)] public float totalHeight = 20;
    [Range(2, 50)] public int columns;
    [Range(2, 50)] public int rows;
    public float tileDelay = 0.005f;
    //public float tilesOffset;

    public GameObject tilePrefab;

    private Tile[,] tiles;

    private void Awake()
    {
        GenerateBoard();
        StartCoroutine(TileAppear());
    }

    private void GenerateBoard()
    {
        tiles = new Tile[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //Vector3 instancePosition = new Vector3(
                //    (float)i * tilePrefab.transform.localScale.x + tilesOffset * i - ((float)(columns - 1) / 2.0f) * tilePrefab.transform.localScale.x - (float)(columns - 1) / 2.0f * tilesOffset,
                //    0f,
                //    (float)j * tilePrefab.transform.localScale.y + tilesOffset * j - ((float)(rows - 1) / 2.0f) * tilePrefab.transform.localScale.y - (float)(rows - 1) / 2.0f * tilesOffset);

                float x = Utility.Interpolate(-totalWidth / 2, totalWidth / 2, 0, columns - 1, i);
                float z = Utility.Interpolate(-totalHeight / 2, totalHeight / 2, 0, rows - 1, j);

                Vector3 position = new Vector3(x, 0f, z);

                Tile newTile = Instantiate<GameObject>(tilePrefab, position, Quaternion.identity, transform).GetComponent<Tile>();
                if (newTile != null)
                {
                    newTile.Coords = new Vector2Int(i, j);
                    tiles[i, j] = newTile;
                    string name = "Tile (" + i + "," + j + ")";
                    newTile.gameObject.name = name;
                    newTile.transform.localScale = new Vector3(totalWidth / (columns - 1), 1f, totalHeight / (rows - 1));
                    //newTile.GetComponentInChildren<TextMeshPro>().text = name;
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

    public void GenerateMap()
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
        foreach(Vector2 v in vectors)
        {
            Tile t = GetTile(v);
            if (t != null)
            {
                tiles.Add(t);
            }
        }
        return tiles;
    }

    public List<Tile> GetTilesBetween(Tile t1, Tile t2)
    {
        List<Tile> between = new List<Tile>();
        if(t1.Equals(t2) || !t1.IsInLine(t2))
        {
            return between;
        }
        if(t1.Coords.x == t2.Coords.x)
        {
            // Search Left
            if(t1.Coords.y > t2.Coords.y)
            {
                for(float i = t1.Coords.y; i>t2.Coords.y && i>=0; i--)
                {
                    between.Add(tiles[(int)(t1.Coords.x), (int)i]);
                }
            }
            // Search Right
            else
            {
                for (float i = t1.Coords.y; i < t2.Coords.y && i < rows; i++)
                {
                    between.Add(tiles[(int)(t1.Coords.x), (int)i]);
                }
            }
        }
        else if (t1.Coords.y == t2.Coords.y)
        {
            // Search Down
            if (t1.Coords.x > t2.Coords.x)
            {
                for (float i = t1.Coords.x; i > t2.Coords.x && i >= 0; i--)
                {
                    between.Add(tiles[(int)i, (int)(t1.Coords.y)]);
                }
            }
            // Search Up
            else
            {
                for (float i = t1.Coords.x; i < t2.Coords.x && i < columns; i++)
                {
                    between.Add(tiles[(int)i, (int)(t1.Coords.y)]);
                }
            }
        }
        else
        {
            if (t1.Coords.y > t2.Coords.y)
            {
                // Search Down Left
                if(t1.Coords.x > t2.Coords.x)
                {
                    for(float i = 1; t1.Coords.x-i > t2.Coords.x && t1.Coords.x - i >= 0 && t2.Coords.x - i >= 0; i--)
                    {
                        between.Add(tiles[(int)(t1.Coords.x - i), (int)(t1.Coords.y - i)]);
                    }
                }
                // Search Down Right
                if (t1.Coords.x < t2.Coords.x)
                {
                    for (float i = 1; t1.Coords.x + i < t2.Coords.x && t1.Coords.x + i < columns && t2.Coords.x + i < rows; i++)
                    {
                        between.Add(tiles[(int)(t1.Coords.x + i), (int)(t1.Coords.y + i)]);
                    }
                }
            }
            else
            {
                // Search Up Left
                if (t1.Coords.x > t2.Coords.x)
                {
                    for (float i = t1.Coords.x; i < t2.Coords.x && i < columns; i++)
                    {
                        between.Add(tiles[(int)i, (int)i]);
                    }
                }
                // Search Up Right
                if (t1.Coords.x < t2.Coords.x)
                {
                    for (float i = t1.Coords.x; i < t2.Coords.x && i > 0; i--)
                    {
                        between.Add(tiles[(int)i, (int)i]);
                    }
                }
            }
        }
        return between;
    }
}
