using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [SerializeField] private int mapWidth = 7;
    [SerializeField] private int mapHeight = 7;
    [SerializeField] private int roomsToGenerate = 12;

    private int roomCount;
    private bool roomsInstantiated;
    private Vector2 firstRoomPos;

    private bool[,] map;
    [SerializeField] private GameObject roomPrefab;

    private List<Room> roomObjects = new List<Room>();

    public static Generation instance;

    // Spawn chances
    [SerializeField] private float enemySpawnChance;
    [SerializeField] private float coinSpawnChance;
    [SerializeField] private float healthSpawnChance;

    // Max number per room
    [SerializeField] private int maxEnemiesPerRoom;
    [SerializeField] private int maxCoinsPerRoom;
    [SerializeField] private int maxHealthPerRoom;

    // Generation properties
    public float EnemySpawnChance { get => enemySpawnChance; }
    public float CoinSpawnChance { get => coinSpawnChance; }
    public float HealthSpawnChance { get => healthSpawnChance; }
    public int MaxEnemiesPerRoom { get => maxEnemiesPerRoom; }
    public int MaxCoinsPerRoom { get => maxCoinsPerRoom; }
    public int MaxHealthPerRoom { get => maxHealthPerRoom; }

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// This function is called when the player moves.  For minimap updating.
    /// </summary>
    public void OnPlayerMove()
    {
        Vector3 playerPos = FindObjectOfType<Player>().transform.position;
        Vector2 roomPos = new Vector2(((int)playerPos.x + 6) / 12, ((int)playerPos.y + 6) / 12);

        UIManager.instance.Map.texture = MapTextureGenerator.Generate(map, roomPos);
    }

    /// <summary>
    /// Begins the generation process.
    /// </summary>
    public void Generate()
    {
        map = new bool[mapWidth, mapHeight];
        CheckRoom(3, 3, 0, Vector2.zero, true);
        InstantiateRooms();
        FindObjectOfType<Player>().transform.position = firstRoomPos * 12;
        UIManager.instance.Map.texture = MapTextureGenerator.Generate(map, firstRoomPos);
    }

    /// <summary>
    /// Handles figuring out where each and every room goes on the map.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="remaining"></param>
    /// <param name="generalDirection"></param>
    /// <param name="firstRoom"></param>
    private void CheckRoom(int x, int y, int remaining, Vector2 generalDirection, bool firstRoom = false)
    {
        if(roomCount >= roomsToGenerate)
        {
            return;
        }

        if(x < 0 || x > mapWidth - 1 || y < 0 || y > mapHeight - 1)
        {
            return;
        }

        if(firstRoom == false && remaining <= 0)
        {
            return;
        }

        if(map[x, y] == true)
        {
            return;
        }

        if(firstRoom == true)
        {
            firstRoomPos = new Vector2(x, y);
        }

        roomCount++;
        map[x, y] = true;

        bool north = Random.value > (generalDirection == Vector2.up ? 0.2f : 0.8f);
        bool south = Random.value > (generalDirection == Vector2.down ? 0.2f : 0.8f);
        bool west = Random.value > (generalDirection == Vector2.left ? 0.2f : 0.8f);
        bool east = Random.value > (generalDirection == Vector2.right ? 0.2f : 0.8f);

        int maxRemaining = roomsToGenerate / 4;

        if(north || firstRoom)
        {
            CheckRoom(x, y + 1, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.up : generalDirection);
        }
        if (south || firstRoom)
        {
            CheckRoom(x, y - 1, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.down : generalDirection);
        }
        if (west || firstRoom)
        {
            CheckRoom(x - 1, y, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.left : generalDirection);
        }
        if (east || firstRoom)
        {
            CheckRoom(x + 1, y, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.right : generalDirection);
        }
    }

    /// <summary>
    /// Spawns in the respective Room prefabs correctly based on what CheckRooms() did.
    /// </summary>
    private void InstantiateRooms()
    {
        if (roomsInstantiated)
        {
            return;
        }

        roomsInstantiated = true;

        for(int x = 0; x < mapWidth; ++x)
        {
            for(int y = 0; y < mapHeight; ++y)
            {
                if(map[x, y] == false)
                {
                    continue;
                }

                GameObject roomObj = Instantiate(roomPrefab, new Vector3(x, y, 0) * 12, Quaternion.identity);
                Room room = roomObj.GetComponent<Room>();

                // Checks for a room to the north.
                if(y < mapHeight - 1 && map[x, y + 1] == true)
                {
                    room.NorthDoor.gameObject.SetActive(true);
                    room.NorthWall.gameObject.SetActive(false);
                }

                // Checks for a room to the south.
                if (y > 0 && map[x, y - 1] == true)
                {
                    room.SouthDoor.gameObject.SetActive(true);
                    room.SouthWall.gameObject.SetActive(false);
                }

                if (x > 0 && map[x - 1, y] == true)
                {
                    room.WestDoor.gameObject.SetActive(true);
                    room.WestWall.gameObject.SetActive(false);
                }

                if (x < mapWidth - 1 && map[x + 1, y] == true)
                {
                    room.EastDoor.gameObject.SetActive(true);
                    room.EastWall.gameObject.SetActive(false);
                }

                if(firstRoomPos != new Vector2(x, y))
                {
                    room.GenerateInterior();
                }

                roomObjects.Add(room);
            }
        }

        CalculateKeyAndExit();
    }

    /// <summary>
    /// Handles figuring out where the key and door are for each Room. Makes sure they are as far apart as possible.
    /// </summary>
    private void CalculateKeyAndExit()
    {
        float maxDistance = 0;
        Room a = null;
        Room b = null;

        foreach (Room aRoom in roomObjects)
        {
            foreach (Room bRoom in roomObjects)
            {
                float distance = Vector3.Distance(aRoom.transform.position, bRoom.transform.position);

                if(distance > maxDistance)
                {
                    a = aRoom;
                    b = bRoom;
                    maxDistance = distance;
                }
            }
        }

        a.SpawnPrefab(a.KeyPrefab);
        b.SpawnPrefab(b.ExitDoorPrefab);
    }
}