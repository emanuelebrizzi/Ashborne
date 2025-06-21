using System.Collections;
using UnityEngine;

public class AttackState : EnemyState
{
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 1.0f;
    float lastAttackTime = -100f;
    AttackHitbox hitbox;

    public override void Enter()
    {
        base.Enter();
        hitbox = GetComponentInChildren<AttackHitbox>();
        Attack();
    }

    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }

    public override void Tick()
    {
        if (!CanAttackPlayer())
        {
            enemy.ChangeState(enemy.ChasingState);
        }
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        enemy.PlayAnimation(Enemy.AnimationState.ATTACK);
        if (hitbox != null)
            hitbox.Activate();

        lastAttackTime = Time.time;
        StartCoroutine(WaitAndReturnToChase());
    }

    IEnumerator WaitAndReturnToChase()
    {
        Animator animator = enemy.GetComponentInChildren<Animator>();
        float waitTime = 0.5f;
        if (animator != null)
        {
            yield return null;
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("ATTACK"))
                waitTime = stateInfo.length;
        }
        yield return new WaitForSeconds(waitTime);
        enemy.ChangeState(enemy.ChasingState);
    }

    bool CanAttackPlayer()
    {
        return Vector2.Distance(enemy.transform.position, Player.Instance.transform.position) <= attackRange;
    }
}
