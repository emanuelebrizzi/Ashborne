using System.Collections;
using UnityEngine;

public class ChasingState : EnemyState
{
    [SerializeField] float maxChaseDistance = 3.0f;
    [SerializeField] float attackRange = 2.0f;
    Vector2 enemyStartingPoint;
    AttackHitbox hitbox;
    float lastAttackTime = -100f;
    readonly float attackCooldown = 1.0f;
    float distanceFromStart;

    public override void Enter()
    {
        base.Enter();
        enemyStartingPoint = enemy.transform.position;
        hitbox = GetComponentInChildren<AttackHitbox>();
        Debug.Log("Enemy entered ChasingState");

        if (nextState == null)
        {
            nextState = GetComponent<PatrolState>();
        }
    }

    public override void Exit()
    {
        StopAllCoroutines();
        base.Exit();
    }

    void FixedUpdate()
    {
        if (IsPlayerTooFar())
        {
            Debug.Log("Player is too far. Exiting chasing state...");
            Exit();
            return;
        }

        Vector2 directionToPlayer = (Player.Instance.transform.position - enemy.transform.position).normalized;
        enemy.MoveInDirection(directionToPlayer.x);

        if (Vector2.Distance(enemy.transform.position, Player.Instance.transform.position) <= attackRange)
        {
            Attack();
        }
    }

    bool IsPlayerTooFar()
    {
        distanceFromStart = Vector2.Distance(enemyStartingPoint, enemy.transform.position);
        return distanceFromStart > maxChaseDistance;
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        StartCoroutine(WaitForAttackAnimationAndCompletion());

        lastAttackTime = Time.time;
    }

    IEnumerator WaitForAttackAnimationAndCompletion()
    {
        // Maybe we can factor out a general method to syncronize actions with the animations' length
        enemy.PlayAnimation(Enemy.AnimationState.ATTACK);
        Animator animator = enemy.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            yield return null;

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("ATTACK"))
            {
                float animationLength = stateInfo.length;

                yield return new WaitForSeconds(animationLength);
            }
        }

        if (hitbox != null)
        {
            hitbox.Activate();
        }
    }
}
