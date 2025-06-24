using System;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected float attackRange = 2.0f;
    [SerializeField] protected float attackCooldown = 1.0f;
    [SerializeField] protected int attackDamage = 10;


    public int AttackDamage => attackDamage;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;

    protected void OnHit(Collider2D target)
    {
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(attackDamage);
            Debug.Log($"Attack: Dealt {attackDamage} damage to {target.name}.");
        }
    }

    public virtual void PerformAttack() { }

    // public void IncreaseDamage(float damageMultiplier)
    // {
    //     damageAmount = Mathf.RoundToInt(damageAmount * damageMultiplier);
    // }

    public void IncreaseAttackDamage(int value)
    {
        attackDamage += value;
    }

    public void IncreaseAttackRange(float value)
    {
        attackRange += value;
    }
}
