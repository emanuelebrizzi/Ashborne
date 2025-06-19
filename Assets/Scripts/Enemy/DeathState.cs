using System;
using System.Collections;
using UnityEngine;

public class DeathState : EnemyState
{
    const float CorpseRemainingTime = 3.0f;
    public event Action OnEnemyDeath;

    public override void Enter()
    {
        base.Enter();

        ProcessDeath();
    }


    void ProcessDeath()
    {
        enemy.SetPhysicElementsTo(false);
        enemy.PlayAnimation(Enemy.AnimationState.DEATH);
        AwardAshEchoes();
        StartCoroutine(WaitForCorpseDisappears());
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

    IEnumerator WaitForCorpseDisappears()
    {
        yield return new WaitForSeconds(CorpseRemainingTime);

        OnEnemyDeath?.Invoke();
    }
}
