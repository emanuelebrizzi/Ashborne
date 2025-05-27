using UnityEngine;

public class PatrolState : EnemyState
{
    Vector2 target;

    [SerializeField] Transform pointA, pointB;
    [SerializeField] float detectionRange = 5f;
    bool goingToB = true;

    public override void Enter()
    {
        base.Enter();
        enemy.MyLogger.Log(Enemy.LoggerTAG, "Entered in the PatrolState");
        target = pointB.position;
        goingToB = true;
    }

    void FixedUpdate()
    {
        float targetX = target.x;
        float currentY = enemy.Body.position.y;
        float newX = Mathf.MoveTowards(enemy.Body.position.x, targetX, enemy.Speed * Time.fixedDeltaTime);
        Vector2 newPosition = new(newX, currentY);
        enemy.Body.MovePosition(newPosition);

        if (Mathf.Abs(enemy.Body.position.x - targetX) < 0.1f)
        {
            goingToB = !goingToB;
            target = goingToB ? pointB.position : pointA.position;
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Changed direction towards " + target);
        }

        Vector2 directionToPlayer = (enemy.player.position - enemy.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, directionToPlayer, detectionRange);

        if (hit.collider != null && hit.collider.transform == enemy.player)
        {
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Player spotted, switching to Chasingstate");
            base.Exit();
        }
    }
}
