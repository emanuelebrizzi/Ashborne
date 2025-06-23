using UnityEngine;

public class ChasingState : EnemyState
{
    [SerializeField] float maxChaseDistance = 3.0f;
    Vector2 enemyStartingPoint;
    float distanceFromStart;

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
            enemy.Controller.ChangeState(enemy.Controller.PatrolState);
            return;
        }

        Vector2 directionToPlayer = (Player.Instance.transform.position - enemy.transform.position).normalized;

        if (enemy.CanAttackPlayer())
        {
            enemy.UpdateSpriteDirection(directionToPlayer.x);
            enemy.Controller.ChangeState(enemy.Controller.AttackState);
            return;
        }

        enemy.MoveInDirection(directionToPlayer.x);
    }

    bool IsPlayerTooFar()
    {
        distanceFromStart = Vector2.Distance(enemyStartingPoint, enemy.transform.position);
        return distanceFromStart > maxChaseDistance;
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
