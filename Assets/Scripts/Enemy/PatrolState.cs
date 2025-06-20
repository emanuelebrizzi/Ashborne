using UnityEngine;

public class PatrolState : EnemyState
{
    const float MinimumDistance = 0.1f;
    [SerializeField] float detectionRange = 3f;

    bool goingToB = true;
    Vector2 target;

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered in the PatrolState");

        if (nextState == null)
        {
            nextState = GetComponent<ChasingState>();
        }

        target = enemy.PointB.position;

        // enemy.SetPatrolPoints(enemy.PointA, enemy.PointB);
    }

    void FixedUpdate()
    {
        if (DetectPlayer())
        {
            Debug.Log("Player detected, exiting patrol state");
            base.Exit();
            return;
        }

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        enemy.MoveInDirection(direction.x);

        float distanceToTarget = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(target.x, 0));
        if (distanceToTarget < MinimumDistance)
        {
            SwitchPatrolDirection();
        }
    }

    bool DetectPlayer()
    {
        if (Player.Instance == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer > detectionRange) return false;

        Vector2 raycastOrigin = (Vector2)transform.position + Vector2.up * 0.5f;
        Vector2 directionToPlayer = (Player.Instance.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(
            raycastOrigin,
            directionToPlayer,
            detectionRange,
            enemy.PlayerMask
        );

        if (hit.collider != null)
        {
            return hit.transform == Player.Instance.transform;
        }

        return false;
    }

    void SwitchPatrolDirection()
    {
        goingToB = !goingToB;
        target = goingToB ? enemy.PointB.position : enemy.PointA.position;
    }
}
