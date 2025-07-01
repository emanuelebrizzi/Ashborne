using UnityEngine;

public class ChasingState : EnemyState
{
    const float minimumDistance = 0.1f;


    Transform enemyStartingPoint;

    public override void Enter()
    {
        base.Enter();
        enemyStartingPoint = enemy.transform;
        Debug.Log("Enemy entered ChasingState");
    }

    public override void Update()
    {
        if (enemy.IsOutOfRange(enemyStartingPoint) || enemy.IsNearPointA(minimumDistance) || enemy.IsNearPointB(minimumDistance))
        {
            enemy.StateController.TransitionTo(enemy.StateController.PatrolState);
            return;
        }

        enemy.MoveToward(Player.Instance.transform);
        enemy.CheckMovementAnimation();

        if (enemy.CanAttackPlayer())
        {
            enemy.StateController.TransitionTo(enemy.StateController.AttackState);
            return;
        }
    }
}
