using System.Collections;
using UnityEngine;

public class AttackState : EnemyState
{
    bool isAttacking = false;
    Coroutine currentAttackRoutine;

    public override void Tick()
    {
        if (!enemy.CanAttackPlayer())
        {
            enemy.Controller.ChangeState(enemy.Controller.ChasingState);
            return;
        }

        Vector2 directionToPlayer = (Player.Instance.transform.position - enemy.transform.position).normalized;
        if (!isAttacking)
        {
            enemy.UpdateSpriteDirection(directionToPlayer.x);
            currentAttackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        enemy.Animator.PlayAnimation(EnemyAnimator.AnimationState.ATTACK);
        enemy.Attack.PerformAttack();

        yield return null;

        float animationLength = enemy.Animator.GetAnimationLength(EnemyAnimator.AnimationState.ATTACK);
        float waitTime = Mathf.Max(animationLength, enemy.Attack.AttackCooldown);

        yield return new WaitForSeconds(waitTime);

        isAttacking = false;
        currentAttackRoutine = null;
    }

    public void CancelAttack()
    {
        if (isAttacking && currentAttackRoutine != null)
        {
            StopCoroutine(currentAttackRoutine);
            currentAttackRoutine = null;
            isAttacking = false;
            enemy.Animator.PlayAnimation(EnemyAnimator.AnimationState.IDLE);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (currentAttackRoutine != null)
        {
            StopCoroutine(currentAttackRoutine);
            currentAttackRoutine = null;
        }

        isAttacking = false;
    }
}
