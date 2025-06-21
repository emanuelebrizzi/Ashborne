using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] Projectile projectile;
    [SerializeField] Transform firePoint;

    public override void PerformAttack()
    {
        Vector2 targetPos = Player.Instance.transform.position;

        if (Player.Instance.TryGetComponent<Collider2D>(out var collider))
            targetPos = collider.bounds.center;

        Vector2 direction = (targetPos - (Vector2)firePoint.position).normalized;
        Projectile proj = Instantiate(projectile, firePoint.position, Quaternion.identity);
        proj.direction = direction;
        proj.OnHit += OnHit;
    }
}
