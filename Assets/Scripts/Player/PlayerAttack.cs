using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Attack Settings")]
    [SerializeField] float attackRange = 1f;
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackCooldown = 0.3f;
    [SerializeField] float attackDelay = 0.4f;
    [SerializeField] LayerMask enemyLayer; // Layer mask to identify enemies
    Transform attackPoint; // Point from where the attack is initiated
    float nextAttackTime = 0f;
    bool isAttacking = false;

    void Start()
    {
        attackPoint = transform.Find("AttackPoint");
    }


    void Update()
    {
        if (isAttacking || Time.time < nextAttackTime)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        Player.Instance.PlayAnimation(PlayerState.ATTACK, 0);
        StartCoroutine(PerformAttackAfterDelay());
    }

    private IEnumerator PerformAttackAfterDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(attackDamage);
            }
        }

        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    public void IncreaseFlatDamage(int value)
    {
        attackDamage += value;
    }
    public void IncreaseFlatRange(float value)
    {
        attackRange += value;
    }

}
