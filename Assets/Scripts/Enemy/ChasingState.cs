using System.Collections;
using UnityEngine;

public class ChasingState : EnemyState
{
    [SerializeField] float maxChaseDistance = 3.0f;
    [SerializeField] float attackRange = 2.0f;
    Vector2 enemyStartingPoint;
    AttackHitbox hitbox;
    float lastAttackTime = -100f;
    [SerializeField] float attackCooldown = 1.0f;
    float distanceFromStart;

    [SerializeField] bool isMage = false;
    [SerializeField] GameObject projectilePrefab; // Assign in Inspector
    [SerializeField] Transform firePoint;

    public override void Enter()
    {
        base.Enter();
        enemyStartingPoint = enemy.transform.position;
        hitbox = GetComponentInChildren<AttackHitbox>();
        Debug.Log("Enemy entered ChasingState");
    }

    public override void Exit()
    {
        StopAllCoroutines();
        base.Exit();
    }

    public override void Tick()
    {
        if (IsPlayerTooFar() || IsEnemyNearBounds())
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }

        Vector2 directionToPlayer = (Player.Instance.transform.position - enemy.transform.position).normalized;
        if (isMage)
        {
            enemy.MoveInDirection(0);
        }
        else
        {
            enemy.MoveInDirection(directionToPlayer.x);

        }

        if (!IsEnemyNearBounds() && CanAttackPlayer())
        {
            Attack();
        }
    }

    bool IsPlayerTooFar()
    {
        // distanceFromStart = Vector2.Distance(enemyStartingPoint, enemy.transform.position);
        // return distanceFromStart > maxChaseDistance;

        if (isMage)
        {
            // Mage: use distance to player
            return Vector2.Distance(enemy.transform.position, Player.Instance.transform.position) > maxChaseDistance;
        }
        else
        {
            // Melee: use distance from starting point
            distanceFromStart = Vector2.Distance(enemyStartingPoint, enemy.transform.position);
            return distanceFromStart > maxChaseDistance;
        }
    }

    bool IsEnemyNearBounds()
    {
        const float minimumDistance = 0.1f;

        if (IsNearPointA(minimumDistance) || IsNearPointB(minimumDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsNearPointA(float minimumDistance)
    {
        return Vector2.Distance(transform.position, enemy.PointA.position) < minimumDistance;
    }

    bool IsNearPointB(float minimumDistance)
    {
        return Vector2.Distance(transform.position, enemy.PointB.position) < minimumDistance;
    }

    bool CanAttackPlayer()
    {
        return Vector2.Distance(enemy.transform.position, Player.Instance.transform.position) <= attackRange;
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        if (isMage)
        {
            StartCoroutine(MageAttackCoroutine());
        }
        else
        {
            StartCoroutine(WaitForAttackAnimationAndCompletion());
        }

        lastAttackTime = Time.time;
    }

    IEnumerator MageAttackCoroutine()
    {
        enemy.PlayAnimation(Enemy.AnimationState.ATTACK);
        Animator animator = enemy.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            yield return null;
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("ATTACK"))
            {
                float animationLength = stateInfo.length;
                yield return new WaitForSeconds(animationLength * 0.5f); // Fire in the middle of the animation
            }
        }

        Vector2 playerPos = Player.Instance.transform.position;
        Vector2 playerVelocity = Player.Instance.GetComponent<Rigidbody2D>().linearVelocity;
        float projectileSpeed = 5f;
        float marginOfError = 1.5f;
        // Predict where the player will be
        float distance = Vector2.Distance(firePoint.position, playerPos);
        float timeToReach = distance / projectileSpeed;
        Vector2 predictedPos = playerPos + playerVelocity * timeToReach;

        // Add random aim error
        Vector2 aimError = Random.insideUnitCircle * marginOfError;
        Vector2 finalTarget = predictedPos + aimError;

        // Calculate direction
        Vector2 direction = (finalTarget - (Vector2)firePoint.position).normalized;

        // Spawn and set up projectile
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        if (proj.TryGetComponent<Projectile>(out var projScript))
        {
            projScript.direction = direction;
            projScript.speed = projectileSpeed;
        }

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
