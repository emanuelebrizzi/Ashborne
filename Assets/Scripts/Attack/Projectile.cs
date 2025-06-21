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
        UpdateSpriteDirection(direction.x);
    }

    void FixedUpdate()
    {
        transform.Translate(speed * Time.fixedDeltaTime * direction.normalized, Space.World);
    }

    public void UpdateSpriteDirection(float directionX)
    {
        if (directionX > 0 && isFacingLeft)
        {
            Flip();
        }
        else if (directionX < 0 && !isFacingLeft)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        isFacingLeft = !isFacingLeft;
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
