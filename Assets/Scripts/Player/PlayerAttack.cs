using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    static readonly float AttackCooldown = 0.5f;
    [SerializeField] Player player;
    [SerializeField] LayerMask enemyLayer; // Layer mask to identify enemies

    [SerializeField] float attackRange = 1f;
    [SerializeField] int attackDamage = 1;
    Transform attackPoint; // Point from where the attack is initiated
    float waitingTime;

    void Start()
    {
        waitingTime = AttackCooldown;
        attackPoint = transform.Find("AttackPoint");

        if (player == null)
        {
            player = GetComponent<Player>();
        }
    }


    void Update()
    {
        if (waitingTime <= 0f && Input.GetKeyDown(KeyCode.Q))
        {
            Attack();
            waitingTime = AttackCooldown;
        }
        else
        {
            waitingTime -= Time.deltaTime;
        }
    }

    void Attack()
    {
        player.PlayAnimation(PlayerState.ATTACK, 1);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(attackDamage);
            }
        }

    }

}
