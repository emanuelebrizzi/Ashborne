using UnityEngine;

public class PatrolState : EnemyState
{
    const float MinimumDistance = 0.1f;
    Transform target;
    bool goingToB = true;

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered in the PatrolState");
        target = enemy.PointB;
    }

    public override void Update()
    {
        if (enemy.HasDetectedPlayer())
        {
            Debug.Log("Player detected, exiting patrol state");
            enemy.StateController.TransitionTo(enemy.StateController.ChasingState);
            return;
        }

        enemy.MoveToward(target);
        enemy.CheckMovementAnimation();

        if (enemy.DistanceTo(target) < MinimumDistance)
        {
            SwitchPatrolDirection();
        }
    }

    void SwitchPatrolDirection()
    {
        goingToB = !goingToB;
        target = goingToB ? enemy.PointB : enemy.PointA;
    }
}
