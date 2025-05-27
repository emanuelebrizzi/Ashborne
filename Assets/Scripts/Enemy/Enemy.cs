using UnityEngine;

[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(ChasingState))]

public class Enemy : MonoBehaviour
{
    public const string LoggerTAG = "Enemy";
    public Logger MyLogger { get; private set; }
    public Rigidbody2D Body { get; private set; }
    public float Speed => speed;
    public Transform player;

    [SerializeField] int healthPoints;
    [SerializeField] float speed = 5.0f;
    [SerializeField] EnemyState initialState;

    PatrolState patrolling;
    ChasingState chasing;

    void Start()
    {
        MyLogger = new Logger(Debug.unityLogger.logHandler);
        Body = GetComponent<Rigidbody2D>();
        patrolling = GetComponent<PatrolState>();
        chasing = GetComponent<ChasingState>();

        ResetState();
    }

    void ResetState()
    {
        patrolling.Exit();
        chasing.Exit();

        if (initialState != null)
        {
            initialState.Enter();
        }
    }


}
