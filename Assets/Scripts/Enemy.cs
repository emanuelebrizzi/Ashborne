using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Enemy : MonoBehaviour
{
    private const string EnemyTAG = "Enemy";
    private Logger myLogger;
    [SerializeField] private int healthPoints = 3;
    [SerializeField] private Rigidbody2D body;

    void Start()
    {
        myLogger = new Logger(Debug.unityLogger.logHandler);
        myLogger.Log(EnemyTAG, "Spawned with health " + healthPoints);
        body = GetComponent<Rigidbody2D>();
    }

}
