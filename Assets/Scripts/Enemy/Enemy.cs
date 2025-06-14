using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(ChasingState))]

public class Enemy : MonoBehaviour
{
    const float DeathTime = 2.0f;
    [SerializeField] float speed = 5.0f;
    [SerializeField] EnemyState initialState;
    [SerializeField] AttackHitbox hitbox;
    [SerializeField] int ashEchoesReward = 100;

    readonly float attackCooldown = 1.0f;
    float lastAttackTime = -100f;
    PatrolState patrolling;
    ChasingState chasing;
    SPUM_Prefabs spumPrefabs;
    bool isDead = false;
    Health health;

    public const string LoggerTAG = "Enemy";


    public Logger MyLogger { get; private set; }
    public Rigidbody2D Body { get; private set; }
    public float Speed => speed;


    void Start()
    {
        MyLogger = new Logger(Debug.unityLogger.logHandler);
        Body = GetComponent<Rigidbody2D>();
        spumPrefabs = GetComponent<SPUM_Prefabs>();
        patrolling = GetComponent<PatrolState>();
        chasing = GetComponent<ChasingState>();
        health = GetComponent<Health>();
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

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
        MyLogger.Log(LoggerTAG, $"Got {damage} damage, remaining {health.CurrentHealth} HP.");

        if (health.CurrentHealth <= 0)
        {
            Die();
            MyLogger.Log(LoggerTAG, "Dying...");
            return;
        }

        spumPrefabs.PlayAnimation(PlayerState.DAMAGED, 0);
    }

    void Die()
    {
        isDead = true;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (patrolling != null) patrolling.enabled = false;
        if (chasing != null) chasing.enabled = false;

        spumPrefabs.PlayAnimation(PlayerState.DEATH, 0);

        AwardAshEchoes();
        Destroy(gameObject, DeathTime);
    }

    void AwardAshEchoes()
    {
        if (Player.Instance != null)
        {
            Player.Instance.AddAshEchoes(ashEchoesReward);
            MyLogger.Log(LoggerTAG, $"Awarded {ashEchoesReward} Ash Echoes to player");
        }
        else
        {
            MyLogger.LogWarning(LoggerTAG, "Player singleton not available. Cannot award Ash Echoes.");

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
        if (patrolling != null) patrolling.Exit();
        if (chasing != null) chasing.Exit();

        if (initialState != null)
        {
            initialState.Enter();
        }
    }
}
