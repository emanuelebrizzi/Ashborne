using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(ChasingState))]

public class Enemy : MonoBehaviour
{
    [SerializeField] int healthPoints;
    [SerializeField] float speed = 5.0f;
    [SerializeField] EnemyState initialState;
    [SerializeField] AttackHitbox hitbox;

    readonly float attackCooldown = 1.0f;
    float lastAttackTime = -100f;
    PatrolState patrolling;
    ChasingState chasing;
    SPUM_Prefabs spumPrefabs;
    bool isDead = false;

    public const string LoggerTAG = "Enemy";
    public Logger MyLogger { get; private set; }
    public Rigidbody2D Body { get; private set; }
    public float Speed => speed;
    public Transform player;


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
        if (!isDead)
            spumPrefabs.PlayAnimation(PlayerState.MOVE, 0);
    }

    public void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        spumPrefabs.PlayAnimation(PlayerState.ATTACK, 0);
        StartCoroutine(PerformAttack());

        lastAttackTime = Time.time;
    }

    IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(0.3f);

        if (hitbox != null)
        {
            hitbox.Activate();
        }
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
        MyLogger.Log(LoggerTAG, $"Enemy took {damage} damage. Remaining HP: {healthPoints}");

        spumPrefabs.PlayAnimation(PlayerState.DAMAGED, 0);
        if (healthPoints <= 0)
        {
            Die();
            MyLogger.Log(LoggerTAG, "Enemy has died.");
        }
    }

    void Die()
    {
        isDead = true;
        if (patrolling != null) patrolling.enabled = false;
        if (chasing != null) chasing.enabled = false;

        spumPrefabs.PlayAnimation(PlayerState.DEATH, 0);
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
    }
}
