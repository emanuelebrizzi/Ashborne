using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform target;

    public override void PerformAttack()
    {
        Vector2 targetPos;

        if (target == null)
            target = Player.Instance.transform;

        if (target.TryGetComponent<Collider2D>(out var collider))
            targetPos = collider.bounds.center;
        else
            targetPos = target.position;

        Vector2 direction = (targetPos - (Vector2)firePoint.position).normalized;

        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        projectile.SetLifetime(attackRange);
        projectile.direction = direction;
        projectile.OnHit += OnHit;
    }
}
