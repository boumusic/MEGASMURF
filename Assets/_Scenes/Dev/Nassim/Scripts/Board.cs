using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public static Board Instance { get { if (!instance) instance = FindObjectOfType<Board>(); return instance; } set { } }

    private static Board instance;

    public int maxX;
    public int maxY;
    public float tilesOffset;

    public GameObject tilePrefab;

    private Tile[,] tiles;

    private void Awake()
    {
        if (Board.Instance != null)
        {
            Board.Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tiles = new Tile[maxX, maxY];
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                Vector3 instancePosition = new Vector3(
                    (float)i * tilePrefab.transform.localScale.x + tilesOffset * i - (((float)(maxX-1.0f) / 2.0f) + tilesOffset) * tilePrefab.transform.localScale.x,
                    (float)j * tilePrefab.transform.localScale.y + tilesOffset * j - (((float)(maxY-1.0f) / 2.0f) + tilesOffset) * tilePrefab.transform.localScale.y,
                    0f);
                Instantiate<GameObject>(tilePrefab, instancePosition, Quaternion.identity, transform).GetComponent<Tile>().Coords = new Vector2(i, j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {

    }

    public Tile GetTile(float x, float y)
    {
        if ((tiles.GetLength(0) - 1) < x || (tiles.GetLength(1) - 1) < y)
        {
            return null;
        }
        else
        {
            return tiles[(int)x, (int)y];
        }
    }

}
