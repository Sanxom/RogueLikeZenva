using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Door Objects")]
    [SerializeField] private Transform northDoor;
    [SerializeField] private Transform southDoor;
    [SerializeField] private Transform westDoor;
    [SerializeField] private Transform eastDoor;

    [Header("Wall Objects")]
    [SerializeField] private Transform northWall;
    [SerializeField] private Transform southWall;
    [SerializeField] private Transform westWall;
    [SerializeField] private Transform eastWall;

    [Header("Values")]
    [SerializeField] private int insideWidth;
    [SerializeField] private int insideHeight;

    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject exitDoorPrefab;

    private List<Vector3> usedPositions = new List<Vector3>();

    public Transform NorthDoor { get => northDoor; set => northDoor = value; }
    public Transform SouthDoor { get => southDoor; set => southDoor = value; }
    public Transform WestDoor { get => westDoor; set => westDoor = value; }
    public Transform EastDoor { get => eastDoor; set => eastDoor = value; }
    public Transform NorthWall { get => northWall; set => northWall = value; }
    public Transform SouthWall { get => southWall; set => southWall = value; }
    public Transform WestWall { get => westWall; set => westWall = value; }
    public Transform EastWall { get => eastWall; set => eastWall = value; }
    public GameObject KeyPrefab { get => keyPrefab; }
    public GameObject ExitDoorPrefab { get => exitDoorPrefab; }

    /// <summary>
    /// Creates all of the interior objects in a room.
    /// </summary>
    public void GenerateInterior()
    {
        // Checks for enemy spawning.
        if(Random.value < Generation.instance.EnemySpawnChance)
        {
            SpawnPrefab(enemyPrefab, 1, Generation.instance.MaxEnemiesPerRoom + 1);
        }
        
        if(Random.value < Generation.instance.CoinSpawnChance)
        {
            SpawnPrefab(coinPrefab, 1, Generation.instance.MaxCoinsPerRoom + 1);
        }

        if(Random.value < Generation.instance.HealthSpawnChance)
        {
            SpawnPrefab(healthPrefab, 1, Generation.instance.MaxHealthPerRoom + 1);
        }
    }

    /// <summary>
    /// Spawns objects in the room.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SpawnPrefab(GameObject prefab, int min = 0, int max = 0)
    {
        int num = 1;

        if(min != 0 || max != 0)
        {
            num = Random.Range(min, max);
        }

        for(int x = 0; x < num; ++x)
        {
            GameObject obj = Instantiate(prefab);
            Vector3 position = transform.position + new Vector3(Random.Range(-insideWidth / 2, insideWidth / 2 + 1), Random.Range(-insideHeight / 2, insideHeight / 2 + 1), 0);

            while (usedPositions.Contains(position))
            {
                position = transform.position + new Vector3(Random.Range(-insideWidth / 2, insideWidth / 2 + 1), Random.Range(-insideHeight / 2, insideHeight / 2 + 1), 0);
            }

            obj.transform.position = position;
            usedPositions.Add(position);

            if(prefab == enemyPrefab)
            {
                EnemyManager.instance.Enemies.Add(obj.GetComponent<Enemy>());
            }
        }
    }
}
