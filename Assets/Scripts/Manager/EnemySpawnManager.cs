using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    // TODO: transform in an array when we have mages and others
    public GameObject enemyPrefab;

    [Serializable]
    public class SpawnPoint
    {
        public Transform position;
        public Transform patrolPointA;
        public Transform patrolPointB;
        public float respawnDelay = 5f;
    }

    ObjectPool<GameObject> enemyPool;
    [SerializeField] SpawnPoint[] spawnPoints;
    Transform enemiesParent; // It's only used for hierarchy management purpose
    Dictionary<SpawnPoint, Enemy> activeEnemies = new();

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

        enemyPool = new ObjectPool<GameObject>(
            OnEnemyCreate,
            OnTake,
            OnRelease,
            OnObjectDestroy,
            maxSize: spawnPoints.Length
        );
    }

    GameObject OnEnemyCreate()
    {
        GameObject enemyObj = Instantiate(enemyPrefab, enemiesParent);
        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.GetComponent<DeathState>().OnEnemyDeath += () => HandleDeathOf(enemy);
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

        if (enemyObj.TryGetComponent<Enemy>(out var enemy) && enemy.MySpawnPoint != null)
        {
            if (activeEnemies.ContainsKey(enemy.MySpawnPoint) && activeEnemies[enemy.MySpawnPoint] == enemy)
            {
                activeEnemies[enemy.MySpawnPoint] = null;
            }
        }
    }

    void OnObjectDestroy(GameObject enemyObj)
    {
        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.GetComponent<DeathState>().OnEnemyDeath -= () => HandleDeathOf(enemy);
        }

        Destroy(enemyObj);
    }

    public void SpawnAllEnemies()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            SpawnEnemyAt(spawnPoint);
        }
    }

    void SpawnEnemyAt(SpawnPoint spawnPoint)
    {
        if (activeEnemies.ContainsKey(spawnPoint) && activeEnemies[spawnPoint] != null && activeEnemies[spawnPoint].gameObject.activeInHierarchy)
            return;

        GameObject enemyObj = enemyPool.Get();
        enemyObj.transform.SetPositionAndRotation(spawnPoint.position.position, spawnPoint.position.rotation);

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.MySpawnPoint = spawnPoint;
        enemy.PointA = spawnPoint.patrolPointA;
        enemy.PointB = spawnPoint.patrolPointB;
        enemy.ResetFroomPool();

        activeEnemies[spawnPoint] = enemy;
        Debug.Log($"Enemy {enemy.Id} spawned!");
    }

    public void HandleDeathOf(Enemy enemy)
    {
        enemyPool.Release(enemy.gameObject);
    }

}
