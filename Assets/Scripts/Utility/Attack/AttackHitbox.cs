using System;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] float hitboxActiveTime = 1.0f;

    bool canDealDamage = false;

    public event Action<Collider2D> OnHit;


    public void Activate()
    {
        canDealDamage = true;
        Invoke(nameof(Deactivate), hitboxActiveTime);
    }

    public void Deactivate()
    {
        canDealDamage = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canDealDamage) return;

        OnHit?.Invoke(other);
        canDealDamage = false;
    }
}
