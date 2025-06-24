using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Attack Settings")]
    [SerializeField] float attackDelay = 0.2f;

    float nextAttackTime = 0f;
    bool isAttacking = false;

    void Update()
    {
        if (isAttacking || Time.time < nextAttackTime)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartAttack();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            RangedAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + Player.Instance.MeleeAttack.AttackCooldown;
        Player.Instance.PlayAnimation(PlayerState.ATTACK, 0);
        Player.Instance.MeleeAttack.PerformAttack();
        StartCoroutine(PerformAttackAfterDelay());
    }


    private void RangedAttack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + Player.Instance.RangedAttack.AttackCooldown;
        Player.Instance.PlayAnimation(PlayerState.ATTACK, 0);
        Player.Instance.RangedAttack.PerformAttack();
        StartCoroutine(PerformAttackAfterDelay());
    }

    private IEnumerator PerformAttackAfterDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
    }
}
