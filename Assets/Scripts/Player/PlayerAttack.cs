using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] static float COOLDOWN_TIME = 0.5f; // Cooldown time in seconds
    SPUM_Prefabs spumPrefabs;

    [SerializeField] LayerMask enemyLayer; // Layer mask to identify enemies
    Transform attackPoint; // Point from where the attack is initiated
    float attackCooldown = COOLDOWN_TIME;
    float lastAttackTime = 0f;
    [SerializeField] float attackRange = 1f; // Range of the attack
    [SerializeField] int attackDamage = 1; // Damage dealt by the attack

    void Start()
    {
        spumPrefabs.OverrideControllerInit();
        attackPoint = transform.Find("AttackPoint");
    }

    void Awake()
    {
        spumPrefabs = GetComponent<SPUM_Prefabs>();
    }

    void Update()
    {
        if (attackCooldown <= 0f && Input.GetKeyDown(KeyCode.Q))
        {
            Attack();
            lastAttackTime = Time.time;
            attackCooldown = COOLDOWN_TIME; // Reset cooldown
        }
        else
        {
            attackCooldown -= Time.deltaTime; // Decrease cooldown over time
        }
    }

    void Attack()
    {
        spumPrefabs.PlayAnimation(PlayerState.ATTACK, 1);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
 
    }

}   
