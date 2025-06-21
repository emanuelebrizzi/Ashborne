using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 3f;
    public event Action<Collider2D> OnHit;
    public Vector2 direction;

    void FixedUpdate()
    {
        transform.Translate(speed * Time.fixedDeltaTime * direction.normalized, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Hit: " + collision.gameObject.name);
            OnHit?.Invoke(collision);
            OnHit = null;
        }

        Destroy(gameObject);
    }

}
