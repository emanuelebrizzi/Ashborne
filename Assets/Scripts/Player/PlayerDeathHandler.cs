using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] HeroSpawnManager heroSpawnManager;
    [SerializeField] GameObject heroObject;

    public void HandleDeath(Transform deathPoint)
    {
        DropAshEchoes(deathPoint);
        Respawn();
    }

    void Respawn()
    {
        heroSpawnManager.SpawnHero();
        Player.Instance.Heal();
    }

    void DropAshEchoes(Transform dropPosition)
    {
        AshEchoes ashEchoes = Player.Instance.AshEchoes;

        if (ashEchoes != null)
        {
            ashEchoes.DropEchoes(dropPosition);
        }
    }

}

