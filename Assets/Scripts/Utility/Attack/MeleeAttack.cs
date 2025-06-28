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

    public override void CancelAttack()
    {
        hitbox.Deactivate();
    }
}
