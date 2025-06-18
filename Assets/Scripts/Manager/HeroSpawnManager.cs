using UnityEngine;

public class HeroSpawnManager : MonoBehaviour
{
    public static HeroSpawnManager Instance;

    [SerializeField] private Transform defaultSpawnPoint;
    private Transform currentCheckpoint;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public void SpawnHero(GameObject heroPrefab)
    {
        Vector3 spawnPos = currentCheckpoint != null ? currentCheckpoint.position : defaultSpawnPoint.position;
        Instantiate(heroPrefab, spawnPos, Quaternion.identity);
    }

    public Vector3 GetRespawnPosition()
    {
        return currentCheckpoint != null ? currentCheckpoint.position : defaultSpawnPoint.position;
    }
}
