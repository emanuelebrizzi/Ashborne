using UnityEngine;

public class HeroSpawnManager : MonoBehaviour
{
    [SerializeField] Transform heroSpawnPoint;
    [SerializeField] GameObject heroPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnHero();
    }

    public void DespawnHero()
    {
        if (heroPrefab == null)
        {
            Debug.LogError("Hero prefab is not set.");
            return;
        }

        heroPrefab.SetActive(false);
    }

    public void SpawnHero()
    {
        SpawnHeroInPosition(heroSpawnPoint);
    }
    public void SpawnHeroInPosition(Transform position)
    {
        if (heroPrefab == null)
        {
            Debug.LogError("Hero prefab is not set.");
            return;
        }

        heroPrefab.transform.position = new Vector3(position.position.x, position.position.y, 0);
        heroPrefab.SetActive(true);
    }

    public void SetHeroSpawnPoint(Transform newSpawnPoint)
    {
        heroSpawnPoint = newSpawnPoint;
    }

}
