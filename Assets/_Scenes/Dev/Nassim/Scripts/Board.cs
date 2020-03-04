using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{

    public static Board Instance;// {
    /*get 
    {
        if (Board.Instance == null)
        {
            Board.Instance = FindObjectOfType<Board>();
        }
        return Board.Instance; 
    }         
    set 
    {
        Instance = value;
    }
}*/

    //private static Board instance;

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
        StartCoroutine(GenerateBoard());
    }

    private IEnumerator GenerateBoard()
    {
        if (Board.Instance == null)
        {
            Board.Instance = this;
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
                        newTile.transform.localScale = new Vector3(totalWidth / (columns-1), 1f, totalHeight / (rows-1));
                        //newTile.GetComponentInChildren<TextMeshPro>().text = name;
                    }

                    yield return new WaitForSeconds(tileDelay);
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

    public Tile GetTile(Vector2Int v)
    {
        return GetTile(v.x, v.y);
    }

}
