using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    /* 
        The current flow updates the health and notifies the changes to the subscribers.
    */
    [SerializeField] int maxHealth = 100;
    int currentHealth = 0;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

}