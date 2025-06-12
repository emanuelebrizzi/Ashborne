using UnityEngine;

public class PatrolState : EnemyState
{
    Vector2 target;

    [SerializeField] Transform pointA, pointB;
    [SerializeField] float detectionRange = 3f;
    [SerializeField] LayerMask playerLayerMask;
    bool goingToB = true;

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

        if (playerLayerMask == 0)
        {
            playerLayerMask = LayerMask.GetMask("Player");
            Debug.Log($"Player layer mask initialized to: {playerLayerMask}");
        }
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

        if (Mathf.Abs(enemy.Body.position.x - targetX) < 0.1f)
        {
            goingToB = !goingToB;
            target = goingToB ? pointB.position : pointA.position;
        }
    }

    bool DetectPlayer()
    {
        if (enemy.player == null) return false;


        float distanceToPlayer = Vector2.Distance(transform.position, enemy.player.position);
        if (distanceToPlayer > detectionRange) return false;

        Vector2 directionToPlayer = (enemy.player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(
        transform.position,
        directionToPlayer,
        detectionRange,
        playerLayerMask
    );

        if (hit.collider != null)
        {
            Debug.Log($"Raycast hit: {hit.collider.name}, looking for player: {enemy.player.name}");
            return hit.transform == enemy.player;
        }

        return false;
    }
}
