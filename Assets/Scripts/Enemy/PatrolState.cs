using UnityEngine;

public class PatrolState : EnemyState
{
    const float MinimumDistance = 0.1f;
    [SerializeField] float detectionRange = 3f;
    LayerMask playerLayerMask;
    bool goingToB = true;
    Transform pointA, pointB;
    Vector2 target;

    public override void Enter()
    {
        base.Enter();
        enemy.MyLogger.Log(Enemy.LoggerTAG, "Entered in the PatrolState");
        enemy.PlayAnimation(PlayerState.MOVE, 0);

        if (nextState == null)
        {
            nextState = GetComponent<ChasingState>();
        }

        playerLayerMask = LayerMask.GetMask("Player");
    }

    void FixedUpdate()
    {
        if (enemy == null || enemy.Body == null)
        {
            return;
        }

        if (DetectPlayer())
        {
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Player detected, switching to Chasingstate");
            base.Exit();
            return;
        }

        float targetX = target.x;
        float currentY = enemy.Body.position.y;
        float newX = Mathf.MoveTowards(enemy.Body.position.x, targetX, enemy.Speed * Time.fixedDeltaTime);
        Vector2 newPosition = new(newX, currentY);
        enemy.Body.MovePosition(newPosition);

        float directionX = goingToB ? 1 : -1;
        enemy.UpdateSpriteDirection(directionX);

        if (Mathf.Abs(enemy.Body.position.x - targetX) < MinimumDistance)
        {
            goingToB = !goingToB;
            target = goingToB ? pointB.position : pointA.position;
        }
    }

    bool DetectPlayer()
    {
        if (Player.Instance == null) return false;

        enemy.MyLogger.Log(Enemy.LoggerTAG, $"Checking for player. Distance: {Vector2.Distance(transform.position, Player.Instance.transform.position)}, Detection range: {detectionRange}");

        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer > detectionRange) return false;

        Vector2 directionToPlayer = (Player.Instance.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(
        transform.position,
        directionToPlayer,
        detectionRange,
        playerLayerMask
    );

        if (hit.collider != null)
        {
            enemy.MyLogger.Log(Enemy.LoggerTAG, $"Raycast hit: {hit.collider.name}, looking for player: {Player.Instance.name}");
            return hit.transform == Player.Instance.transform;
        }

        return false;
    }

    public void SetPatrolPoints(Transform pointA, Transform pointB)
    {
        this.pointA = pointA;
        this.pointB = pointB;

        target = pointB.position;
        goingToB = true;
    }
}
