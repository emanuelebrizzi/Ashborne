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
            StartCoroutine(PerformAttackAfterDelay());
            Player.Instance.MeleeAttack.PerformAttack();
        }
    }

    public void RangedAttack()
    {
        if (!Player.Instance.RangedAttack.enabled)
        {
            Debug.Log("Not again unlocked!");
            return;
        }

        if (!(isAttacking || Time.time < nextAttackTime))
        {
            isAttacking = true;
            nextAttackTime = Time.time + Player.Instance.RangedAttack.AttackCooldown;
            StartCoroutine(PerformAttackAfterDelay());
            Player.Instance.RangedAttack.PerformAttack();
        }

    }

    IEnumerator PerformAttackAfterDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
    }

}
