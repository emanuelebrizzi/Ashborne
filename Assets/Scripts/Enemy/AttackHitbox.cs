using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float hitboxActiveTime = 1.0f;

    private bool canDealDamage = false;

    public void Activate()
    {
        canDealDamage = true;
        Invoke(nameof(Deactivate), hitboxActiveTime);
    }

    public void Deactivate()
    {
        canDealDamage = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    void CheckCollision(Collider2D other)
    {
        if (!canDealDamage)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            if (Player.Instance != null)
            {
                Debug.Log($"Applying {damageAmount} damage to player");
                Player.Instance.TakeDamage(damageAmount);
                canDealDamage = false;
            }
            else
            {
                Debug.LogWarning("Player object doesn't have a Health component!");
            }
        }
    }
}
