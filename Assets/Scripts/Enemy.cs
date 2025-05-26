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
        Vector2 direction = (target - body.position).normalized;
        body.linearVelocity = new Vector2(direction.x * speed, body.linearVelocity.y);

        if (Vector2.Distance(body.position, target) < 0.5f)
        {
            goingToB = !goingToB;
            target = goingToB ? pointB.position : pointA.position;
            myLogger.Log(EnemyTAG, "Changed direction towards " + target);
            myLogger.Log(EnemyTAG, "velocity is " + body.linearVelocity.ToString());
        }

    }
}
