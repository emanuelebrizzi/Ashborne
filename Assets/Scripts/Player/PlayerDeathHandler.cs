using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] HeroSpawnManager heroSpawnManager;
    [SerializeField] GameObject heroObject;

    public void Die()
    {
        DropAshEchoes(heroObject.transform);
        Respawn();
    }

    public void FallDeath(Transform fallDeathEchoDropPoint)
    {
        DropAshEchoes(fallDeathEchoDropPoint);
        Respawn();
    }

    void Respawn()
    {
        heroSpawnManager.SpawnHero();
        Player.Instance.Health.ResetHealth();
    }
    void DropAshEchoes(Transform dropPosition)
        {
        AshEchoes ashEchoes = Player.Instance.AshEchoes;

        if (ashEchoes != null)
        {
            ashEchoes.DropEchoes(dropPosition);
        }
        else
        {
            Debug.LogWarning("AshEchoes component not found on hero. No echoes dropped.");
        }
    }

}

