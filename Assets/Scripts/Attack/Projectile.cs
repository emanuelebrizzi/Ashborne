using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    bool isFacingLeft = true;
    public Vector2 direction;
    public event Action<Collider2D> OnHit;

    void Start()
    {
        EntityUtility.FlipSpriteHorizontally(transform, direction.x, ref isFacingLeft);
    }

    void FixedUpdate()
    {
        transform.Translate(speed * Time.fixedDeltaTime * direction.normalized, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnHit?.Invoke(collision);
            OnHit = null; // Force unsubscription
        }

        Destroy(gameObject);
    }

}
