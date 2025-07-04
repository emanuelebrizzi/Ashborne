using System;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected int attackDamage = 10;
    [SerializeField] protected float attackRange = 2.0f;
    [SerializeField] protected float attackCooldown = 1.0f;


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

    public abstract void PerformAttack();

    public virtual void CancelAttack() { }

    public void IncreaseAttackDamage(int value)
    {
        attackDamage += value;
    }

    public void IncreaseAttackRange(float value)
    {
        attackRange += value;
    }
}
