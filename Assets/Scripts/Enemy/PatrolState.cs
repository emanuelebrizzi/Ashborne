using UnityEngine;

public class PatrolState : EnemyState
{
    Vector2 target;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Transform pointA, pointB;
    [SerializeField] float detectionRange = 5f;
    bool goingToB = true;

    public override void Enter()
    {
        base.Enter();
        enemy.MyLogger.Log(Enemy.LoggerTAG, "Entered in the PatrolState");
        if (body == null)
        {
            body = enemy.GetComponent<Rigidbody2D>();
            if (body == null)
            {
                enemy.MyLogger.LogError(Enemy.LoggerTAG, "Rigidbody2D component is missing on the GameObject.");
                return;
            }
        }
        target = pointB.position;
        goingToB = true;
    }

    void FixedUpdate()
    {
        float targetX = target.x;
        float currentY = body.position.y;
        float newX = Mathf.MoveTowards(body.position.x, targetX, enemy.Speed * Time.fixedDeltaTime);
        Vector2 newPosition = new(newX, currentY);
        body.MovePosition(newPosition);

        if (Mathf.Abs(body.position.x - targetX) < 0.1f)
        {
            goingToB = !goingToB;
            target = goingToB ? pointB.position : pointA.position;
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Changed direction towards " + target);
        }

        Vector2 directionToPlayer = (enemy.player.position - enemy.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, directionToPlayer, detectionRange);
        Debug.DrawRay(enemy.transform.position, directionToPlayer * detectionRange, Color.yellow); // Visualize the ray
        if (hit.collider != null)
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Raycast hit: " + hit.collider.name);

        if (hit.collider != null && hit.collider.transform == enemy.player)
        {
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Player spotted, switching to Chasingstate");
            base.Exit();
        }
    }
}
