using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    [HideInInspector] public Vector2 direction = Vector2.right;

    void Update()
    {
        UpdateSpriteDirection(direction.x);
        transform.Translate(speed * Time.deltaTime * direction.normalized, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.Instance.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    public void UpdateSpriteDirection(float directionX)
    {
        if (directionX != 0)
        {
            float sign = Mathf.Sign(directionX);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -sign, transform.localScale.y, transform.localScale.z);
        }
    }
}
