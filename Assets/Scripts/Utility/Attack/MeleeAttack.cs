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

    public void CancelAttack()
    {
        hitbox.Deactivate();
    }
}
