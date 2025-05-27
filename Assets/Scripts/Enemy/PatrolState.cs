using UnityEngine;

public class PatrolState : EnemyState
{
    Vector2 target;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Transform pointA, pointB;
    bool goingToB = true;

    public override void Enter()
    {
        base.Enter();
        enemy.MyLogger.Log(Enemy.LoggerTAG, "Entered in the PatrolState");
        body = GetComponent<Rigidbody2D>();
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
    }
}
