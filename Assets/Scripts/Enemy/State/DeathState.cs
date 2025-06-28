using System;
using System.Collections;
using UnityEngine;

public class DeathState : EnemyState
{
    const float CorpseRemainingTime = 2.0f;
    public event Action OnEnemyDeath;

    public override void Enter()
    {
        base.Enter();
        enemy.AwardAshEchoes();
    }

    public override void Update()
    {
        enemy.Play(Animations.DEATH, true, true);
        EntityUtility.SetPhysicsEnabled(gameObject, false);
        StartCoroutine(WaitForCorpseDisappears());
    }

    IEnumerator WaitForCorpseDisappears()
    {
        yield return new WaitForSeconds(CorpseRemainingTime);

        OnEnemyDeath?.Invoke();
    }
}
