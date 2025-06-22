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
    public class EnemyConfiguration
    {
        public EnemyType type;
        public GameObject prefab;
        public SpawnPoint[] spawnPoints;
    }

    [SerializeField] List<EnemyConfiguration> enemyConfigurations = new();
    Transform enemiesParent; // It's only used for hierarchy management purpose
    readonly Dictionary<EnemyType, ObjectPool<GameObject>> enemyPools = new();
    readonly Dictionary<EnemyType, Dictionary<SpawnPoint, Enemy>> activeEnemies = new();

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

        foreach (var config in enemyConfigurations)
        {
            // Create pool for this enemy type
            enemyPools[config.type] = new ObjectPool<GameObject>(
                () => OnEnemyCreate(config),
                OnTake,
                OnRelease,
                (enemyObj) => OnObjectDestroy(enemyObj, config),
                maxSize: config.spawnPoints.Length
            );

            // Init functions for tracking
            activeEnemies[config.type] = new Dictionary<SpawnPoint, Enemy>();
            foreach (var spawnPoint in config.spawnPoints)
            {
                activeEnemies[config.type][spawnPoint] = null;
            }
        }

        if (GameManager.Instance != null)
            GameManager.Instance.RegisterEnemySpawnManager(this);
    }

    GameObject OnEnemyCreate(EnemyConfiguration configuration)
    {
        GameObject enemyObj = Instantiate(configuration.prefab, enemiesParent);

        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.GetComponent<DeathState>().OnEnemyDeath += () => HandleDeathOf(enemy, configuration.type);
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

    public void SpawnAllEnemies()
    {
        foreach (var config in enemyConfigurations)
        {
            foreach (var spawnPoint in config.spawnPoints)
            {
                SpawnEnemyAt(config.type, spawnPoint);
            }
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
        SpawnPoint associatedSpawnPoint = null;

        foreach (var kvp in activeEnemies[type])
        {
            if (kvp.Value == enemy)
            {
                associatedSpawnPoint = kvp.Key;
                activeEnemies[type][associatedSpawnPoint] = null;
                break;
            }
        }

        enemyPools[type].Release(enemy.gameObject);
        Debug.Log($"Enemy {enemy.Id} (Type: {type}) died!");
    }

    void OnObjectDestroy(GameObject enemyObj, EnemyConfiguration configuration)
    {
        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.GetComponent<DeathState>().OnEnemyDeath -= () => HandleDeathOf(enemy, configuration.type);
        }

        Destroy(enemyObj);
    }

}
