using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PatrolState))]
public class Enemy : MonoBehaviour
{
    [SerializeField] int healthPoints;
    [SerializeField] float speed = 5.0f;
    public Logger MyLogger { get; private set; }
    public float Speed => speed;

    public const string LoggerTAG = "Enemy";
    PatrolState patrolling;
    void Start()
    {
        MyLogger = new Logger(Debug.unityLogger.logHandler);
        patrolling = GetComponent<PatrolState>();

        ResetState();
    }

    void ResetState()
    {
        patrolling.Exit();
        patrolling.Enter();
    }


}
