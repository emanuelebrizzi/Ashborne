using System.Collections;
using UnityEngine;

public class DeathState : EnemyState
{
    const float corpseRemaingTime = 2.0f;

    bool hasProcessedDeath = false;

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

        enemy.PlayAnimation(PlayerState.DEATH, 0);
        AwardAshEchoes();
        StartCoroutine(RemoveCorpseAfterDelay());
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

    private IEnumerator RemoveCorpseAfterDelay()
    {
        yield return new WaitForSeconds(corpseRemaingTime);

        // Either disable or destroy based on respawn settings
        // if (GameManager.Instance != null && GameManager.Instance.ShouldRespawnEnemies)
        // {
        //     gameObject.SetActive(false);
        //     enemy.MyLogger.Log(Enemy.LoggerTAG, "Enemy disabled for future respawn");
        // }
        // else
        // {
        //     Destroy(gameObject);
        //     enemy.MyLogger.Log(Enemy.LoggerTAG, "Enemy destroyed");
        // }

        Destroy(gameObject);
        enemy.MyLogger.Log(Enemy.LoggerTAG, "Died");
    }
}
