using UnityEngine;

public class PatrolState : EnemyState
{
    Vector2 target;

    [SerializeField] Transform pointA, pointB;
    [SerializeField] float detectionRange = 3f;
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

        float directionX = goingToB ? 1 : -1;
        enemy.UpdateSpriteDirection(directionX);

        if (Mathf.Abs(enemy.Body.position.x - targetX) < 0.1f)
        {
            goingToB = !goingToB;
            target = goingToB ? pointB.position : pointA.position;
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Changed direction towards " + target);

        }

        Collider2D hit = Physics2D.OverlapCircle(enemy.transform.position, detectionRange);

        if (hit != null && hit.transform == enemy.player)
        {
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Player detected, switching to Chasingstate");
            base.Exit();
        }
    }


}
