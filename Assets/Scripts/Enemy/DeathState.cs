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

        if (enemy.Body != null)
        {
            enemy.Body.simulated = false;
        }

        enemy.PlayAnimation(Enemy.AnimationState.DEATH);
        AwardAshEchoes();
        OnEnemyDeath?.Invoke();
    }

    void AwardAshEchoes()
    {
        if (Player.Instance != null)
        {
            Player.Instance.AddAshEchoes(enemy.Reward);
            Debug.Log($"Awarded {enemy.Reward} Ash Echoes to player");
        }
        else
        {
            Debug.LogWarning("Player singleton not available. Cannot award Ash Echoes.");

        }
    }
}
