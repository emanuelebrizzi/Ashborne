using System;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected float attackRange = 2.0f;
    [SerializeField] protected float attackCooldown = 1.0f;
    [SerializeField] protected int damageAmount = 10;

    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;

    protected void OnHit(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.Instance.TakeDamage(damageAmount);
            Debug.Log($"RangedAttack: Dealt {damageAmount} damage to player.");
        }
    }

    public virtual void PerformAttack() { }

    public void IncreaseDamage(float damageMultiplier)
    {
        damageAmount = Mathf.RoundToInt(damageAmount * damageMultiplier);
    }
}
