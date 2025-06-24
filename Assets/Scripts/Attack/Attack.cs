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
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damageAmount);
            Debug.Log($"Attack: Dealt {damageAmount} damage to {target.name}.");
        }
    }

    public virtual void PerformAttack() { }

    public void IncreaseDamage(float damageMultiplier)
    {
        damageAmount = Mathf.RoundToInt(damageAmount * damageMultiplier);
    }
}
