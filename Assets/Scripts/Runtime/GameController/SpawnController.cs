using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private GameObject[] _heroPrefab;
    [SerializeField] private GameObject[] _itemPrefab;
    [SerializeField] private GameObject _heroGroup;
    [SerializeField] private GameObject _enemyGroup;
    [SerializeField] private GameObject _itemGroup;

    public int startSpawnHeroMin = 0; // Random spawn hero min per times
    public int startSpawnHeroMax = 0; // Random spawn hero max per times
    public int chanceSpawnHeroPercent = 0; // Percent chance of spawn
    public int limitSpawnHero = 0; // Limit spawn hero

    public int startSpawnEnemyMin = 0; // Random spawn enemy min per times
    public int startSpawnEnemyMax = 0; // Random spawn enemy max per times
    public int chanceSpawnEnemyPercent = 0; // Percent chance of spawn
    public int limitSpawnEnemy = 0; // Limit spawn enemy

    public int startSpawnItemMin = 0; // Random spawn item min per times
    public int startSpawnItemMax = 0; // Random spawn item max per times

    public void Init(Tilemap obstacle, Tilemap trap)
    {
        SetSpawnEnemy(obstacle, trap);
        SetSpawnPotion(obstacle, trap);
        SpawnRandomEnemy(_heroPrefab, _heroGroup, startSpawnHeroMin, startSpawnHeroMax, obstacle, trap);
    }

    public void SetSpawnHero(Tilemap obstacle, Tilemap trap)
    {
        bool success = Helper.GetRandomWithPercent(chanceSpawnHeroPercent);
        if (success)
        {
            SpawnRandomHero(_heroPrefab, _heroGroup, startSpawnHeroMin, startSpawnHeroMax, obstacle, trap);
        }
        else
        {
            SpawnRandomHero(_heroPrefab, _heroGroup, 1, 1, obstacle, trap);
        }
    }

    public void SetSpawnEnemy(Tilemap obstacle, Tilemap trap)
    {
        bool success = Helper.GetRandomWithPercent(chanceSpawnEnemyPercent);
        if (success)
        {
            SpawnRandomEnemy(_enemyPrefab, _enemyGroup, startSpawnEnemyMin, startSpawnEnemyMax, obstacle, trap);
        }
        else
        {
            SpawnRandomEnemy(_enemyPrefab, _enemyGroup, 1, 1, obstacle, trap);
        }
    }

    public void SetSpawnPotion(Tilemap obstacle, Tilemap trap)
    {
        Transform item = _itemGroup.transform;
        for (int i = 0; i < item.childCount; i++)
        {
            Destroy(item.GetChild(i).gameObject);
        }

        SpawnEntities(_itemPrefab, _itemGroup, obstacle, trap);
    }

    public void EnemyLevelUp(int levelStage)
    {
        Transform enemy = _enemyGroup.transform;
        for (int i = 0; i < enemy.childCount; i++)
        {
            int levelStatusMax = 5;
            EnemyClass enemyClass = enemy.GetChild(i).gameObject.GetComponent<EnemyClass>();
            enemyClass.attack += Helper.GetRandomRang(0, levelStage % levelStatusMax);
        }
    }

    private void SpawnRandomEnemy(GameObject[] prefabs, GameObject group, int spawnMin, int spawnMax, Tilemap obstacle, Tilemap trap)
    {
        int randomSpawn = Helper.GetRandomRang(spawnMin, spawnMax);
        for (int i = 0; i < randomSpawn; i++)
        {
            if (_enemyGroup.transform.childCount >= limitSpawnEnemy) break;
            SpawnEntities(prefabs, group, obstacle, trap);
        }
    }

    private void SpawnRandomHero(GameObject[] prefabs, GameObject group, int spawnMin, int spawnMax, Tilemap obstacle, Tilemap trap)
    {
        int randomSpawn = Helper.GetRandomRang(spawnMin, spawnMax);
        for (int i = 0; i < randomSpawn; i++)
        {
            if (_heroGroup.transform.childCount >= limitSpawnEnemy) break;
            SpawnEntities(prefabs, group, obstacle, trap);
        }
    }

    private void SpawnEntities(GameObject[] prefabs, GameObject group, Tilemap obstacle, Tilemap trap)
    {
        Vector3Int randomPosition = GetValidPosition(obstacle, trap);
        int randomPrefabIndex = Helper.GetRandomRang(0, prefabs.Length - 1);
        GameObject entity = Instantiate(prefabs[randomPrefabIndex], randomPosition, Quaternion.identity);
        entity.transform.parent = group.transform;
    }

    private Vector3Int GetValidPosition(Tilemap obstacle, Tilemap trap)
    {
        Vector3Int randomPosition;
        TileBase obstacleTile;
        TileBase trapTile;

        do
        {
            int gridX = Helper.GetRandomRang(-7, 6);
            int gridY = Helper.GetRandomRang(-7, 6);
            randomPosition = new Vector3Int(gridX, gridY, 0);

            obstacleTile = obstacle.GetTile(randomPosition);
            trapTile = trap.GetTile(randomPosition);

        } while (obstacleTile != null || trapTile != null);

        return randomPosition;
    }
}
