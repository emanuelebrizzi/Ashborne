using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Attack Settings")]
    [SerializeField] float attackDelay = 0.2f;

    float nextAttackTime = 0f;
    bool isAttacking = false;
    public void StartAttack()
    {
        if (!(isAttacking || Time.time < nextAttackTime))
        {  
            isAttacking = true;
            nextAttackTime = Time.time + Player.Instance.MeleeAttack.AttackCooldown;
            Player.Instance.PlayAnimation(PlayerState.ATTACK, 0);
            Player.Instance.MeleeAttack.PerformAttack();
            StartCoroutine(PerformAttackAfterDelay());
        }
    }

    public void RangedAttack()
    {
        if (!(isAttacking || Time.time < nextAttackTime))
        {
            isAttacking = true;
            nextAttackTime = Time.time + Player.Instance.RangedAttack.AttackCooldown;
            Player.Instance.PlayAnimation(PlayerState.ATTACK, 0);
            Player.Instance.RangedAttack.PerformAttack();
            StartCoroutine(PerformAttackAfterDelay());
        }

    }

    IEnumerator PerformAttackAfterDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
    }
}
