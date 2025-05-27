using UnityEngine;

[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(ChasingState))]

public class Enemy : MonoBehaviour
{
    [SerializeField] int healthPoints;
    [SerializeField] float speed = 5.0f;
    [SerializeField] EnemyState initialState;

    public Transform player;
    public Logger MyLogger { get; private set; }
    public float Speed => speed;

    public const string LoggerTAG = "Enemy";
    PatrolState patrolling;
    ChasingState chasing;

    void Start()
    {
        MyLogger = new Logger(Debug.unityLogger.logHandler);
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
