using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    const int SecondWaveEchoesThreshold = 200;
    const int EmpowermentEchoesThreshold = 500;

    [Header("Waves Settings")]
    [SerializeField] List<EnemyWave> enemyWaves = new();

    [Header("Empowerment Settings")]
    [SerializeField] float damageMultiplier = 1.5f;
    [SerializeField] float healthMultiplier = 1.3f;
    [SerializeField] float speedMultiplier = 1.2f;

    Transform enemiesParent; // It's only used for hierarchy management
    readonly Dictionary<EnemyType, ObjectPool<GameObject>> enemyPools = new();
    readonly Dictionary<EnemyType, Dictionary<SpawnPoint, Enemy>> activeEnemies = new();
    bool secondWaveSpawned = false;
    bool enemiesEmpowered = false;

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

    public static EnemySpawnManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Only used for hierarchy management
        var enemiesParentObject = new GameObject("Enemies");
        enemiesParent = enemiesParentObject.transform;

        foreach (var wave in enemyWaves)
        {
            SetupEnemyObjectPool(wave);
            SetupActiveEnemies(wave);
        }

        if (GameManager.Instance != null)
            GameManager.Instance.RegisterEnemySpawnManager(this);


    }

    void SetupEnemyObjectPool(EnemyWave wave)
    {
        enemyPools[wave.type] = new ObjectPool<GameObject>(
                () => OnEnemyCreate(wave),
                OnTake,
                OnRelease,
                (enemyObj) => OnObjectDestroy(enemyObj, wave),
                maxSize: wave.spawnPoints.Length
            );
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

    void OnObjectDestroy(GameObject enemyObj, EnemyWave configuration)
    {
        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.GetComponent<DeathState>().OnEnemyDeath -= () => HandleDeathOf(enemy, configuration.type);
        }

        Destroy(enemyObj);
    }

    void SetupActiveEnemies(EnemyWave wave)
    {
        activeEnemies[wave.type] = new Dictionary<SpawnPoint, Enemy>();
        foreach (var spawnPoint in wave.spawnPoints)
        {
            activeEnemies[wave.type][spawnPoint] = null;
        }
    }

    void Start()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnEchoesChanged += CheckEchoesThreshold;
            Player.Instance.OnDeath += SpawnAllWaves;
        }
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
            SpawnEnemyAt(spawnPoint, wave.type);
        }
    }

    void SpawnEnemyAt(SpawnPoint spawnPoint, EnemyType type)
    {
        if (HasAlreadySpawnedAt(spawnPoint, type))
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

    bool HasAlreadySpawnedAt(SpawnPoint spawnPoint, EnemyType type)
    {
        return activeEnemies[type].ContainsKey(spawnPoint) &&
        activeEnemies[type][spawnPoint] != null &&
        activeEnemies[type][spawnPoint].gameObject.activeInHierarchy;
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

    void CheckEchoesThreshold(int echoesAmount)
    {
        if (!secondWaveSpawned && echoesAmount >= SecondWaveEchoesThreshold)
        {
            secondWaveSpawned = true;
            Spawn(enemyWaves[1]);
            Debug.Log($"Player reached {SecondWaveEchoesThreshold} echoes! Spawning second wave!");
        }

        if (!enemiesEmpowered && echoesAmount >= EmpowermentEchoesThreshold)
        {
            EmpowerAllEnemies();
            enemiesEmpowered = true;
            Debug.Log($"Player reached {EmpowermentEchoesThreshold} echoes! All enemies empowered!");
        }
    }

    void EmpowerAllEnemies()
    {
        foreach (var typeDict in activeEnemies)
        {
            foreach (var kvp in typeDict.Value)
            {
                Enemy enemy = kvp.Value;
                if (enemy != null && enemy.gameObject.activeInHierarchy)
                {
                    enemy.Empower(damageMultiplier, healthMultiplier, speedMultiplier);
                }
            }
        }
    }

    void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnEchoesChanged -= CheckEchoesThreshold;
        }
    }
}
