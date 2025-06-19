using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{

    // TODO: transform in an array
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

    public static EnemySpawnManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

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
        GameObject enemy = Instantiate(enemyPrefab);
        return enemy;
    }

    void OnTake(GameObject enemyObj)
    {
        enemyObj.SetActive(true);

        if (enemyObj.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.ResetFroomPool();
            enemy.GetComponent<DeathState>().OnEnemyDeath += () => HandleRespawnOf(enemy);
        }
    }

    void OnRelease(GameObject enemyObj)
    {
        enemyObj.SetActive(false);
    }

    void OnObjectDestroy(GameObject enemyObj)
    {
        Destroy(enemyObj);
    }

    void Start()
    {
        InitialSpawn();
    }


    void InitialSpawn()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            SpawnEnemyAt(spawnPoint);
        }
    }

    void SpawnEnemyAt(SpawnPoint spawnPoint)
    {
        GameObject enemyObj = enemyPool.Get();
        enemyObj.transform.SetPositionAndRotation(spawnPoint.position.position, spawnPoint.position.rotation);

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.MySpawnPoint = spawnPoint;
        PatrolState patrolState = enemy.GetComponent<PatrolState>();
        patrolState.SetPatrolPoints(spawnPoint.patrolPointA, spawnPoint.patrolPointB);

        Debug.Log($"Enemy {enemy.Id} spawned!");
    }

    public void HandleRespawnOf(Enemy enemy)
    {
        StartCoroutine(RespawnAfterDeleay(enemy));
    }

    IEnumerator RespawnAfterDeleay(Enemy enemy)
    {
        enemyPool.Release(enemy.gameObject);

        yield return new WaitForSeconds(enemy.MySpawnPoint.respawnDelay);

        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
        {
            SpawnEnemyAt(enemy.MySpawnPoint);
        }
    }
}
