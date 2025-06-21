using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 3f;
    public event Action<Collider2D> OnHit;
    public Vector2 direction;
    bool isFacingLeft = true;

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
            Debug.Log("Hit: " + collision.gameObject.name);
            OnHit?.Invoke(collision);
            OnHit = null;
        }

        Destroy(gameObject);
    }

}
