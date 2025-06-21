using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected float attackRange = 2.0f;
    [SerializeField] protected float attackCooldown = 1.0f;
    [SerializeField] protected int damageAmount = 10;

    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;

    public virtual void PerformAttack() { }
}
