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

    SPUM_Prefabs spumPrefabs;

    void Start()
    {
        MyLogger = new Logger(Debug.unityLogger.logHandler);
        Body = GetComponent<Rigidbody2D>();
        spumPrefabs = GetComponent<SPUM_Prefabs>();
        patrolling = GetComponent<PatrolState>();
        chasing = GetComponent<ChasingState>();
        spumPrefabs.OverrideControllerInit();

        ResetState();
    }

    void Update()
    {
        spumPrefabs.PlayAnimation(PlayerState.MOVE, 0);
    }

    public void Attack()
    {
        MyLogger.Log(LoggerTAG, "Enemy is attacking the player!");
        spumPrefabs.PlayAnimation(PlayerState.ATTACK, 0);
    }

    // The assumption is that the sprite is facing left when x is positive
    public void UpdateSpriteDirection(float directionX)
    {
        if (directionX > 0)
        {
            // Moving right
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (directionX < 0)
        {
            // Moving left
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
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

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Die();
            MyLogger.Log(LoggerTAG, "Enemy has died.");
        }
    }

    void Die()
    {
        spumPrefabs.PlayAnimation(PlayerState.DEATH, 0);
        // Additional logic for enemy death can be added here, such as dropping loot or playing a death sound.
        Destroy(gameObject, 1f); // Destroy the enemy after 1 second to allow the death animation to play.
    }
}
