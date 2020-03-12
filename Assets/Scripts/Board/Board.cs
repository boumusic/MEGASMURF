using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

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
    public float tileDelay = 0.0000001f;

    public List<Room> dungeon;

    [SerializeField] private Room debugRoom;
    //public float tilesOffset;

    public GameObject tilePrefab;

    public GameObject skillTree;

    private Tile[,] tiles;

    [HideInInspector]
    public  Maestro maestro;

    public List<GameObject> environments;

    private Room currentRoom;

    private int roomId;

    private GameObject currentEnvironment;

    private List<Tile> exitTiles;
    private List<Tile> spawnTiles;

    [HideInInspector]
    public List<Spawner> spawners;
    [HideInInspector]
    public List<ImmediateSpawner> immediateSpawners;

    public void InitializeBoard()
    {
        InitializeBoard(debugRoom);
    }

    public void InitializeBoard(Room room)
    {
        currentRoom = room;
        currentRoom.OrderElements();
        GenerateTiles();
        StartCoroutine(TileAppear());
        GenerateUnits();
    }

    public void InitializeEnvironment(GameObject environment)
    {
        if (environment != null)
        {
            if (currentEnvironment != null)
            {
                currentEnvironment.SetActive(false);
                currentEnvironment = environment;
                currentEnvironment.SetActive(true);
            }
        }
    }

    public bool SpawnersActive()
    {
        foreach(Spawner s in spawners)
        {
            if (s.activeSpawn)
            {
                return true;
            }
        }
        foreach (ImmediateSpawner s in immediateSpawners)
        {
            if (s.activeSpawn)
            {
                return true;
            }
        }
        return false;
    }

    public void NextRoom()
    {
        roomId++;
        skillTree.SetActive(true);
        ClearRoom();
        if (roomId < dungeon.Count)
        {
            if(roomId < environments.Count)
            {
                InitializeEnvironment(environments[roomId]);
            }
            InitializeBoard(dungeon[roomId]);
        }
        else
        {
            EndDungeon();
        }
    }

    public void NewSpawnersTurn()
    {
        foreach(Spawner s in spawners)
        {
            s.NewTurn();
        }
        foreach (ImmediateSpawner s in immediateSpawners)
        {
            s.NewTurn();
        }
    }

    public void ClearRoom()
    {
        GameManager.units = new List<Unit>();
        foreach(Tile t in exitTiles)
        {
            if (t.unit != null && (t.type == TileType.Ally || t.type == TileType.Obstacle))
            {
                GameManager.units.Add(t.unit);
                t.unit.UnspawnUnit();
            }
        }

        DestroyTiles();

        BattleManager.Instance.playerUnits[0].Clear();
        BattleManager.Instance.playerUnits[1].Clear();
    }

    public void EndDungeon()
    {

    }

    private void DestroyTiles()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if(tiles[i,j].unit != null && !GameManager.units.Contains(tiles[i,j].unit))
                {
                    tiles[i, j].unit.UnspawnUnit();
                }
                tiles[i, j].gameObject.SetActive(false);
            }
        }
    }

    private void GenerateTiles()
    {
        tiles = new Tile[columns, rows];
        spawnTiles = new List<Tile>();
        exitTiles = new List<Tile>();
        spawners = new List<Spawner>();
        immediateSpawners = new List<ImmediateSpawner>();
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                float x = Utility.Interpolate(-totalWidth / 2, totalWidth / 2, 0, columns - 1, i);
                float z = Utility.Interpolate(-totalHeight / 2, totalHeight / 2, 0, rows - 1, j);

                Vector3 position = new Vector3(x, 0f, z);

                LevelElement tile = currentRoom.GetTile(i, j);
                if(tile)
                {
                    Tile newTile = PoolManager.Instance.GetEntityOfType(tile.GetType()) as Tile;
                    if (newTile != null)
                    {
                        newTile.gameObject.SetActive(true);
                        newTile.Coords = new Vector2Int(i, j);
                        newTile.transform.position = position;
                        tiles[i, j] = newTile;
                        if(tiles[i,j] is SpawnTile)
                        {
                            spawnTiles.Add(tiles[i, j]);
                        }
                        else if (tiles[i, j] is ExitTile)
                        {
                            ((ExitTile)tiles[i, j]).id = exitTiles.Count;
                            exitTiles.Add(tiles[i, j]);

                        }
                        string name = "Tile (" + i + "," + j + ")";
                        newTile.gameObject.name = name;
                        newTile.transform.localScale = new Vector3(totalWidth / (columns - 1), 1f, totalHeight / (rows - 1));

                        LevelElement entity = currentRoom.GetEntity(i, j);
                        if (entity)
                        {
                            if (entity is Enemy)
                            {
                                Enemy enemy = PoolManager.Instance.GetEntityOfType(entity.GetType()) as Enemy;
                                if (enemy != null)
                                {
                                    if (tiles[i, j] is ImmediateSpawner)
                                    {
                                        ((ImmediateSpawner)tiles[i,j]).spawnedType = enemy.UnitType;
                                        enemy.gameObject.SetActive(true);
                                        enemy.SpawnUnit(tiles[i, j]);
                                        immediateSpawners.Add((ImmediateSpawner)tiles[i, j]);
                                    }
                                    else if (tiles[i, j] is Spawner)
                                    {
                                        ((Spawner)tiles[i, j]).spawnedType = enemy.UnitType;
                                        spawners.Add(((Spawner)tiles[i, j]));
                                    }
                                    else
                                    {
                                        enemy.gameObject.SetActive(true);
                                        enemy.SpawnUnit(tiles[i, j]);
                                    }
                                }
                            }
                            else
                            {
                                LevelElement newEntity = PoolManager.Instance.GetEntityOfType(entity.GetType()) as LevelElement;

                                if (newEntity != null)
                                {
                                    newEntity.gameObject.SetActive(true);
                                    newEntity.transform.position = position;
                                }
                            }
                        }
                    }
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
        if (maestro == null)
        {
            GameObject maestroObject = UnitFactory.Instance.CreateUnit(BaseUnitType.Maestro);
            if(maestroObject != null)
            {
                maestro = maestroObject.GetComponent<Maestro>();
                maestro.SpawnUnit(spawnTiles[0]);
            }
        }
        if (GameManager.units != null)
        {
            foreach (Unit u in GameManager.units)
            {
                if (spawnTiles.Count > u.SpawnID)
                {
                    u.gameObject.SetActive(true);
                    u.SpawnUnit(spawnTiles[u.SpawnID]);
                }
            }
        }
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

    public List<Tile> GetTilesInLine(Tile tile, Direction dir)
    {
        List<Tile> tiles = new List<Tile>();
        Vector2 v = tile.Coords;
        switch (dir)
        {
            case Direction.Down:
                v = new Vector2(v.x, v.y - 1);
                while(GetTile(v) != null)
                {
                    tiles.Add(GetTile(v));
                    v = new Vector2(v.x, v.y - 1);
                }
                break;
            case Direction.Up:
                v = new Vector2(v.x, v.y + 1);
                while (GetTile(v) != null)
                {
                    tiles.Add(GetTile(v));
                    v = new Vector2(v.x, v.y - 1);
                }
                break;
            case Direction.Left:
                v = new Vector2(v.x - 1, v.y);
                while (GetTile(v) != null)
                {
                    tiles.Add(GetTile(v));
                    v = new Vector2(v.x - 1, v.y);
                }
                break;
            case Direction.Right:
                v = new Vector2(v.x + 1, v.y);
                while (GetTile(v) != null)
                {
                    tiles.Add(GetTile(v));
                    v = new Vector2(v.x + 1, v.y);
                }
                break;
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
