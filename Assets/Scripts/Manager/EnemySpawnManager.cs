using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    [Serializable]
    public enum EnemyType
    {
        Melee,
        Ranged,
    }

    [Serializable]
    public class SpawnPoint
    {
        public Transform position;
        public Transform patrolPointA;
        public Transform patrolPointB;
    }

    [Serializable]
    public class EnemyWave
    {
        public EnemyType type;
        public GameObject prefab;
        public SpawnPoint[] spawnPoints;
    }

    const int EchoesThreshold = 200;
    [SerializeField] List<EnemyWave> enemyWaves = new();
    Transform enemiesParent; // It's only used for hierarchy management purpose
    readonly Dictionary<EnemyType, ObjectPool<GameObject>> enemyPools = new();
    readonly Dictionary<EnemyType, Dictionary<SpawnPoint, Enemy>> activeEnemies = new();
    bool secondWaveSpawned = false;

    public static EnemySpawnManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        var enemiesParentObject = new GameObject("Enemies");
        enemiesParent = enemiesParentObject.transform;

        foreach (var wave in enemyWaves)
        {
            // Create pool for this enemy type
            enemyPools[wave.type] = new ObjectPool<GameObject>(
                () => OnEnemyCreate(wave),
                OnTake,
                OnRelease,
                (enemyObj) => OnObjectDestroy(enemyObj, wave),
                maxSize: wave.spawnPoints.Length
            );

            // Init functions for tracking
            activeEnemies[wave.type] = new Dictionary<SpawnPoint, Enemy>();
            foreach (var spawnPoint in wave.spawnPoints)
            {
                activeEnemies[wave.type][spawnPoint] = null;
            }
        }

        if (GameManager.Instance != null)
            GameManager.Instance.RegisterEnemySpawnManager(this);
    }

    void Start()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnEchoesChanged += CheckEchoesThreshold;
        }
    }

    GameObject OnEnemyCreate(EnemyWave wave)
    {
        GameObject enemyObj = Instantiate(wave.prefab, enemiesParent);

        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.GetComponent<DeathState>().OnEnemyDeath += () => HandleDeathOf(enemy, wave.type);
        }

        return enemyObj;
    }

    void OnTake(GameObject enemyObj)
    {
        enemyObj.SetActive(true);
    }

    void OnRelease(GameObject enemyObj)
    {
        enemyObj.SetActive(false);
    }

    public void SpawnAllWaves()
    {
        foreach (var wave in enemyWaves)
        {
            Spawn(wave);
        }
    }

    void Spawn(EnemyWave wave)
    {
        if (wave.type == EnemyType.Ranged && !secondWaveSpawned)
        {
            return;
        }

        foreach (var spawnPoint in wave.spawnPoints)
        {
            SpawnEnemyAt(wave.type, spawnPoint);
        }
    }

    void SpawnEnemyAt(EnemyType type, SpawnPoint spawnPoint)
    {
        if (activeEnemies[type].ContainsKey(spawnPoint) &&
       activeEnemies[type][spawnPoint] != null &&
       activeEnemies[type][spawnPoint].gameObject.activeInHierarchy)
            return;


        GameObject enemyObj = enemyPools[type].Get();
        enemyObj.transform.SetPositionAndRotation(spawnPoint.position.position, spawnPoint.position.rotation);

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.PointA = spawnPoint.patrolPointA;
        enemy.PointB = spawnPoint.patrolPointB;
        enemy.ResetFroomPool();

        activeEnemies[type][spawnPoint] = enemy;
        Debug.Log($"Enemy {enemy.Id} spawned!");
    }

    public void HandleDeathOf(Enemy enemy, EnemyType type)
    {
        foreach (var kvp in activeEnemies[type])
        {
            if (kvp.Value == enemy)
            {
                SpawnPoint associatedSpawnPoint = kvp.Key;
                activeEnemies[type][associatedSpawnPoint] = null;
                break;
            }
        }

        enemyPools[type].Release(enemy.gameObject);
        Debug.Log($"Enemy {enemy.Id} (Type: {type}) died!");
    }

    void OnObjectDestroy(GameObject enemyObj, EnemyWave configuration)
    {
        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.GetComponent<DeathState>().OnEnemyDeath -= () => HandleDeathOf(enemy, configuration.type);
        }

        Destroy(enemyObj);
    }

    void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnEchoesChanged -= CheckEchoesThreshold;
        }
    }

    void CheckEchoesThreshold(int echoesAmount)
    {
        if (!secondWaveSpawned && echoesAmount >= EchoesThreshold)
        {
            secondWaveSpawned = true;
            Spawn(enemyWaves[1]);
            Debug.Log($"Player reached {EchoesThreshold} echoes! Spawning second wave!");
        }
    }
}
