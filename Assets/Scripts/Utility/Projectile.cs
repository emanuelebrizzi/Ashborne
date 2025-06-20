using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int damage = 10;

    void Start()
    {
        UpdateSpriteDirection(transform.position.x);
    }


    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.Instance.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void UpdateSpriteDirection(float directionX)
    {
        if (directionX != 0)
        {
            float sign = Mathf.Sign(directionX);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * sign, transform.localScale.y, transform.localScale.z);
        }
    }
}
