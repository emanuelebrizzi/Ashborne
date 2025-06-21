using UnityEngine;

public class MeleeAttack : Attack
{
    [SerializeField] AttackHitbox hitbox;

    void Awake()
    {
        hitbox.OnHit += OnHit;
    }

    public override void PerformAttack()
    {
        hitbox.Activate();
    }

    private void OnHit(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.Instance.TakeDamage(damageAmount);
            Debug.Log($"MeleeAttack: Dealt {damageAmount} damage to player.");
        }
    }
}
