using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int maxX;
    public int maxY;
    public float tilesOffset;

    public GameObject tilePrefab;

    private Tile[,] tiles;

    private void Awake()
    {
        if (Board.Instance == null)
        {
            Board.Instance = this;
            tiles = new Tile[maxX, maxY];
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    Vector3 instancePosition = new Vector3(
                        (float)i * tilePrefab.transform.localScale.x + tilesOffset * i - ((float)(maxX - 1) / 2.0f) * tilePrefab.transform.localScale.x - (float)(maxX - 1) / 2.0f * tilesOffset,
                        (float)j * tilePrefab.transform.localScale.y + tilesOffset * j - ((float)(maxY - 1) / 2.0f) * tilePrefab.transform.localScale.y - (float)(maxY - 1) / 2.0f * tilesOffset,
                        0f);
                    Tile newTile = Instantiate<GameObject>(tilePrefab, instancePosition, Quaternion.identity, transform).GetComponent<Tile>();
                    if (newTile != null)
                    {
                        newTile.Coords = new Vector2Int(i, j);
                        tiles[i, j] = newTile;
                    }
                }
            }
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    tiles[i, j].CheckNeighbors();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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

    public Tile GetTile(Vector2 v)
    {
        return GetTile((int)v.x, (int)v.y);
    }

}
