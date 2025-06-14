using UnityEngine;

public class PatrolState : EnemyState
{
    const float MinimumDistance = 0.1f;
    [SerializeField] Transform pointA, pointB;
    [SerializeField] float detectionRange = 3f;
    LayerMask playerLayerMask;
    bool goingToB = true;
    Vector2 target;

    public override void Enter()
    {
        base.Enter();
        enemy.MyLogger.Log(Enemy.LoggerTAG, "Entered in the PatrolState");
        target = pointB.position;
        goingToB = true;

        if (nextState == null)
        {
            nextState = GetComponent<ChasingState>();
        }

        playerLayerMask = LayerMask.GetMask("Player");
    }

    void FixedUpdate()
    {
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
}
