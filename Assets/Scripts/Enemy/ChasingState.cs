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

        if (nextState == null)
        {
            nextState = GetComponent<PatrolState>();
        }
    }

    void FixedUpdate()
    {
        float distanceFromStart = Vector2.Distance(enemyStartingPoint, enemy.transform.position);

        if (distanceFromStart > maxChaseDistance)
        {
            enemy.MyLogger.Log(Enemy.LoggerTAG, "Player is too far, returning to PatrolState");
            base.Exit();
            return;
        }

        Vector2 directionToPlayer = (Player.Instance.transform.position - enemy.transform.position).normalized;

        enemy.UpdateSpriteDirection(directionToPlayer.x);

        Vector2 newPosition = new(
            enemy.transform.position.x + enemy.Speed * Time.fixedDeltaTime * directionToPlayer.x,
            enemy.transform.position.y
        );
        enemy.Body.MovePosition(newPosition);

        if (Vector2.Distance(enemy.transform.position, Player.Instance.transform.position) <= 2.0f)
        {
            enemy.Attack();
        }
    }


}
