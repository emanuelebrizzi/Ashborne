using UnityEngine;

public class ChasingState : EnemyState
{
    [SerializeField] float maxChaseDistance = 3f;
    Vector2 enemyStartingPoint;

    public override void Enter()
    {
        base.Enter();
        enemyStartingPoint = enemy.transform.position;
        enemy.MyLogger.Log(Enemy.LoggerTAG, "Entered ChasingState");
    }

    void FixedUpdate()
    {
        float distanceFromStart = Vector2.Distance(enemyStartingPoint, enemy.transform.position);

        if (distanceFromStart > maxChaseDistance)
        {
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Player is too far, returning to PatrolState");
            base.Exit();
        }

        Vector2 directionToPlayer = (enemy.player.position - enemy.transform.position).normalized;
        Vector2 newPosition = new(
            enemy.transform.position.x + enemy.Speed * Time.fixedDeltaTime * directionToPlayer.x,
            enemy.transform.position.y
        );
        enemy.Body.MovePosition(newPosition);

        enemy.MyLogger.Log(Enemy.LoggerTAG, "Chasing player at position: " + enemy.player.position);
    }
}
