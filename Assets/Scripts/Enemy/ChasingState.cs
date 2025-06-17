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

    public override void Enter()
    {
        base.Enter();
        enemyStartingPoint = enemy.transform.position;
        hitbox = GetComponentInChildren<AttackHitbox>();
        enemy.MyLogger.Log(Enemy.LoggerTAG, "Entered ChasingState");

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
        if (enemy == null || enemy.Body == null || Player.Instance == null)
        {
            return;
        }

        float distanceFromStart = Vector2.Distance(enemyStartingPoint, enemy.transform.position);

        if (distanceFromStart > maxChaseDistance)
        {
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Player is too far, returning to PatrolState");
            base.Exit();
            return;
        }

        enemy.PlayAnimation(PlayerState.MOVE, 0);

        Vector2 directionToPlayer = (Player.Instance.transform.position - enemy.transform.position).normalized;

        enemy.UpdateSpriteDirection(directionToPlayer.x);

        Vector2 newPosition = new(
            enemy.transform.position.x + enemy.Speed * Time.fixedDeltaTime * directionToPlayer.x,
            enemy.transform.position.y
        );
        enemy.Body.MovePosition(newPosition);

        if (Vector2.Distance(enemy.transform.position, Player.Instance.transform.position) <= attackRange)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;


        StartCoroutine(PerformAttack());

        lastAttackTime = Time.time;
    }

    IEnumerator PerformAttack()
    {
        // Maybe we can factor out a general method to syncronize actions with the animations' length
        enemy.PlayAnimation(PlayerState.ATTACK, 0);
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
