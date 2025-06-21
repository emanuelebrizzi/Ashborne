using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] Projectile projectile;
    [SerializeField] Transform firePoint;

    public override void PerformAttack()
    {
        Vector2 direction = (Player.Instance.transform.position - firePoint.position).normalized;
        Projectile proj = Instantiate(projectile, firePoint.position, Quaternion.identity);
        proj.direction = direction;
        proj.OnHit += OnHit;
    }

    void OnHit(Collider2D target)
    {
        Debug.Log("Hitted something");
        if (target.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.Instance.TakeDamage(damageAmount);
            Debug.Log($"RangedAttack: Dealt {damageAmount} damage to player.");
        }
    }
}
