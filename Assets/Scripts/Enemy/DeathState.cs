using System;
using UnityEngine;

public class DeathState : EnemyState
{
    public event Action OnEnemyDeath;

    public override void Enter()
    {
        base.Enter();

        ProcessDeath();
    }

    void ProcessDeath()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // Disable Rigidbody physics
        if (enemy.Body != null)
        {
            enemy.Body.simulated = false;
        }

        enemy.PlayAnimation(PlayerState.DEATH, 0);
        AwardAshEchoes();
        OnEnemyDeath?.Invoke();
    }

    void AwardAshEchoes()
    {
        if (Player.Instance != null)
        {
            Player.Instance.AddAshEchoes(enemy.Reward);
            enemy.MyLogger.Log(Enemy.LoggerTAG, $"Awarded {enemy.Reward} Ash Echoes to player");
        }
        else
        {
            enemy.MyLogger.LogWarning(Enemy.LoggerTAG, "Player singleton not available. Cannot award Ash Echoes.");

        }
    }
}
