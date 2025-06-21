using UnityEngine;

public class ChasingState : EnemyState
{
    [SerializeField] float maxChaseDistance = 3.0f;
    [SerializeField] float attackRange = 2.0f;
    Vector2 enemyStartingPoint;
    float distanceFromStart;

    [SerializeField] bool isMage = false;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    public override void Enter()
    {
        base.Enter();
        enemyStartingPoint = enemy.transform.position;
        Debug.Log("Enemy entered ChasingState");
    }

    public override void Tick()
    {
        if (IsPlayerTooFar() || IsEnemyNearBounds())
        {
            enemy.ChangeState(enemy.PatrolState);
            return;
        }

        Vector2 directionToPlayer = (Player.Instance.transform.position - enemy.transform.position).normalized;

        if (!isMage && Vector2.Distance(enemy.transform.position, Player.Instance.transform.position) <= attackRange)
        {
            enemy.UpdateSpriteDirection(directionToPlayer.x);
            enemy.ChangeState(enemy.AttackState);
            return;
        }


        if (isMage)
        {
            enemy.MoveInDirection(0);
        }
        else
        {
            enemy.MoveInDirection(directionToPlayer.x);

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
}
