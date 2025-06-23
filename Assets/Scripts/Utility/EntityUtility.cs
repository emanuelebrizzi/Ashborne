using UnityEngine;

public static class EntityUtility
{
    public static void SetPhysicsEnabled(GameObject entity, bool value)
    {
        Collider2D[] colliders = entity.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = value;
        }

        if (entity.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.simulated = value;
        }
    }

    public static void FlipSpriteHorizontally(Transform transform, float directionX, ref bool isFacingLeft)
    {
        if (directionX > 0 && isFacingLeft)
        {
            Flip(transform);
            isFacingLeft = false;
        }
        else if (directionX < 0 && !isFacingLeft)
        {
            Flip(transform);
            isFacingLeft = true;
        }

    }

    static void Flip(Transform transform)
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}