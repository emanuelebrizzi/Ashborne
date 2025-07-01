using System.Collections;
using UnityEngine;

public class AttackState : EnemyState
{
    bool isAttacking;

    public override void Enter()
    {
        base.Enter();
        isAttacking = false;
    }

    public override void Update()
    {
        if (!enemy.CanAttackPlayer() && !isAttacking)
        {
            enemy.StateController.TransitionTo(enemy.StateController.ChasingState);
            return;
        }

        if (!isAttacking && enemy.CurrentAnimation != Animations.DAMAGED)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        enemy.Play(Animations.ATTACK, false, false);
        enemy.Attack.PerformAttack();

        yield return new WaitForSeconds(enemy.Attack.AttackCooldown);
        isAttacking = false;
    }

    public override void Exit()
    {

        if (isAttacking)
        {
            StopAllCoroutines();
            isAttacking = false;
        }
        base.Exit();
    }

    public void InterruptAttack()
    {
        if (isAttacking)
        {
            StopAllCoroutines();
            isAttacking = false;
            enemy.Attack.CancelAttack();
        }
    }

}
