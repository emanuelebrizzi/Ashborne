using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const string EnemyTAG = "Enemy";
    private Logger myLogger;
    private Vector2 target;
    [SerializeField] private int healthPoints;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Transform pointA, pointB;
    private bool goingToB = true;

    void Start()
    {
        myLogger = new Logger(Debug.unityLogger.logHandler);
        myLogger.Log(EnemyTAG, "Spawned with health " + healthPoints);
        body = GetComponent<Rigidbody2D>();
        target = pointB.position;
        goingToB = true;
    }

    void FixedUpdate()
    {
        float targetX = target.x;
        float currentY = body.position.y;
        float newX = Mathf.MoveTowards(body.position.x, targetX, speed * Time.fixedDeltaTime);
        Vector2 newPosition = new(newX, currentY);
        body.MovePosition(newPosition);

        if (Mathf.Abs(body.position.x - targetX) < 0.1f)
        {
            goingToB = !goingToB;
            target = goingToB ? pointB.position : pointA.position;
            myLogger.Log(EnemyTAG, "Changed direction towards " + target);
        }

    }
}
