using System;
using System.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField] GameObject enemyPrefab;

    [Serializable]
    public class SpawnPoint
    {
        public string pointId;
        public Transform position;
        public float respawnDelay = 5f;
        [HideInInspector] public bool isOccupied = false;

        public Transform patrolPointA;
        public Transform patrolPointB;
    }

    [SerializeField] SpawnPoint[] spawnPoints;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Register for game state changes
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        SpawnAllEnemies();
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        // You could handle pausing enemy behavior here if needed
    }

    public void SpawnAllEnemies()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (!spawnPoint.isOccupied)
            {
                SpawnEnemyAt(spawnPoint);
            }
        }
    }

    public void SpawnEnemyAt(SpawnPoint spawnPoint)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned in EnemySpawnManager!");
            return;
        }

        GameObject enemyObj = Instantiate(enemyPrefab, spawnPoint.position.position, spawnPoint.position.rotation);

        var enemy = enemyObj.GetComponent<Enemy>();
        if (enemyObj.TryGetComponent<PatrolState>(out var patrolState))
        {
            patrolState.SetPatrolPoints(spawnPoint.patrolPointA, spawnPoint.patrolPointB);
        }
        string spawnId = spawnPoint.pointId;
        enemy.OnEnemyDeath += () => HandleEnemyDeath(spawnId);
        spawnPoint.isOccupied = true;

        Debug.Log($"Enemy spawned at fixed position: {spawnPoint.pointId}");
    }

    public void HandleEnemyDeath(string spawnPointId)
    {
        SpawnPoint spawnPoint = null;
        foreach (var point in spawnPoints)
        {
            if (point.pointId == spawnPointId)
            {
                spawnPoint = point;
                break;
            }
        }

        if (spawnPoint != null)
        {
            spawnPoint.isOccupied = false;
            StartCoroutine(RespawnAfterDelay(spawnPoint));
        }
    }

    private IEnumerator RespawnAfterDelay(SpawnPoint spawnPoint)
    {
        yield return new WaitForSeconds(spawnPoint.respawnDelay);

        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
        {
            SpawnEnemyAt(spawnPoint);
        }
    }

}
