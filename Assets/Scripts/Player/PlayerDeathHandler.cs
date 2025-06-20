using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] HeroSpawnManager heroSpawnManager;
    [SerializeField] Player hero;
    [SerializeField] GameObject heroObject;

    public void Die()
    {
        DropAshEchoes();
        Respawn();
    }
    private void Respawn()
    {
        heroSpawnManager.SpawnHero();
        hero.Health.ResetHealth();
    }

    private void DropAshEchoes()
    {
        AshEchoes ashEchoes = hero.AshEchoes;

        if (ashEchoes != null)
        {
            ashEchoes.DropEchoes(heroObject.transform);
        }
        else
        {
            Debug.LogWarning("AshEchoes component not found on hero. No echoes dropped.");
        }
    }


}

