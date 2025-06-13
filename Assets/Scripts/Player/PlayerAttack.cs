using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    static readonly float AttackCooldown = 0.5f;
    [SerializeField] Player player;
    [SerializeField] LayerMask enemyLayer; // Layer mask to identify enemies

    [Header("Attack Properties")]
    [SerializeField] float attackRange = 1f;
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackDelay = 0.25f;
    Transform attackPoint; // Point from where the attack is initiated
    float waitingTime;
    bool isAttacking = false;


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
        if (isAttacking)
            return;

        if (waitingTime <= 0f && Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(PerformAttack());
            waitingTime = AttackCooldown;
        }
        else
        {
            waitingTime -= Time.deltaTime;
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;

        player.PlayAnimation(PlayerState.ATTACK, 1);
        yield return new WaitForSeconds(attackDelay);
        ApplyDamage();
        yield return new WaitForSeconds(AttackCooldown - attackDelay);

        isAttacking = false;
    }

    void ApplyDamage()
    {
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
