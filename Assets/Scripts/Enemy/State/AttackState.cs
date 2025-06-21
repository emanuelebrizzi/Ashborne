using System.Collections;
using UnityEngine;

public class AttackState : EnemyState
{
    bool isAttacking = false;
    public override void Tick()
    {
        if (!CanAttackPlayer())
        {
            enemy.ChangeState(enemy.ChasingState);
            return;
        }

        Vector2 directionToPlayer = (Player.Instance.transform.position - enemy.transform.position).normalized;
        if (!isAttacking)
        {
            enemy.UpdateSpriteDirection(directionToPlayer.x);
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        enemy.PlayAnimation(Enemy.AnimationState.ATTACK);
        enemy.AttackBehaviour.PerformAttack();

        yield return null;

        float animationLength = GetAnimationTime();
        float waitTime = Mathf.Max(animationLength, enemy.AttackBehaviour.AttackCooldown);

        yield return new WaitForSeconds(waitTime);

        isAttacking = false;
    }

    float GetAnimationTime()
    {
        float animationLength = 0.5f;
        AnimatorStateInfo stateInfo = enemy.Animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("ATTACK"))
            animationLength = stateInfo.length;

        return animationLength;
    }


    bool CanAttackPlayer()
    {
        return Vector2.Distance(enemy.transform.position, Player.Instance.transform.position) <= enemy.AttackBehaviour.AttackRange;
    }

    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
        isAttacking = false;
    }
}
