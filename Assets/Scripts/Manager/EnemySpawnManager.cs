using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }

    [Serializable]
    public class SpawnPoint
    {
        public string pointId; // Maybe make it private because we want it auto generated
        public Transform position;
        public GameObject enemyPrefab;
        public float respawnDelay = 5f;
        public Transform patrolPointA;
        public Transform patrolPointB;
        [HideInInspector] public bool isOccupied = false;
        [HideInInspector] public Enemy activeEnemy = null;
    }

    [SerializeField] SpawnPoint[] spawnPoints;
    private Dictionary<string, SpawnPoint> spawnPointLookup = new();

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
            if (string.IsNullOrEmpty(point.pointId))
            {
                point.pointId = Guid.NewGuid().ToString();
                Debug.LogWarning($"SpawnPoint had no ID. Generated ID: {point.pointId}");
            }

            spawnPointLookup[point.pointId] = point;
        }

        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        InitialSpawn();
    }

    void OnGameStateChanged(GameManager.GameState newState)
    {
        // You could handle pausing enemy behavior here if needed
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
        yield return new WaitForSeconds(3.0f); // Time corpse remains visible

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
