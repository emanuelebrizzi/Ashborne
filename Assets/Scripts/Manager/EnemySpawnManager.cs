using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    const float CorpseRemainingTime = 3.0f;

    [Serializable]
    public class SpawnPoint
    {
        public GameObject enemyPrefab;
        public Transform position;
        public Transform patrolPointA;
        public Transform patrolPointB;
        public float respawnDelay = 5f;
        [HideInInspector] public bool isOccupied = false;
        [HideInInspector] public Enemy activeEnemy = null;
        [HideInInspector] public string pointId;
    }

    [SerializeField] SpawnPoint[] spawnPoints;
    Dictionary<string, SpawnPoint> spawnPointLookup = new();

    public static EnemySpawnManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        foreach (var point in spawnPoints)
        {
            point.pointId = Guid.NewGuid().ToString();
            spawnPointLookup[point.pointId] = point;
        }

        InitialSpawn();
    }


    void InitialSpawn()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (!spawnPoint.isOccupied)
            {
                SpawnEnemyAt(spawnPoint);
            }
        }
    }

    void SpawnEnemyAt(SpawnPoint spawnPoint)
    {
        GameObject enemyObj = Instantiate(spawnPoint.enemyPrefab, spawnPoint.position.position, spawnPoint.position.rotation);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        PatrolState patrolState = enemy.GetComponent<PatrolState>();
        patrolState.SetPatrolPoints(spawnPoint.patrolPointA, spawnPoint.patrolPointB);

        spawnPoint.isOccupied = true;
        spawnPoint.activeEnemy = enemy;
        spawnPoint.activeEnemy.GetComponent<DeathState>().OnEnemyDeath += () => HandleEnemyDeath(spawnPoint.pointId);

        Debug.Log($"Enemy spawned at fixed position: {spawnPoint.pointId}");
    }

    public void HandleEnemyDeath(string spawnPointId)
    {
        if (!spawnPointLookup.TryGetValue(spawnPointId, out SpawnPoint spawnPoint))
        {
            Debug.LogError($"Unknown spawn point ID: {spawnPointId}");
            return;
        }

        StartCoroutine(DestroyAndRespawn(spawnPoint));
    }

    private IEnumerator DestroyAndRespawn(SpawnPoint spawnPoint)
    {
        yield return new WaitForSeconds(CorpseRemainingTime);

        DestroyEnemyAt(spawnPoint);
        spawnPoint.isOccupied = false;

        yield return new WaitForSeconds(spawnPoint.respawnDelay);

        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
        {
            SpawnEnemyAt(spawnPoint);
        }
    }

    void DestroyEnemyAt(SpawnPoint spawnPoint)
    {
        if (spawnPoint.activeEnemy != null)
        {
            Destroy(spawnPoint.activeEnemy.gameObject);
            spawnPoint.activeEnemy = null;
            Debug.Log($"Enemy corpse removed from {spawnPoint.pointId}");
        }
    }
}
