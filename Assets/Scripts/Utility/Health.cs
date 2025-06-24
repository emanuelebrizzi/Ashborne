using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth = 0;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public event Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamaage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void IncreaseMaxHealth(float healthMultiplier)
    {
        int newMaxHealth = Mathf.RoundToInt(maxHealth * healthMultiplier);
        // Increase current health by the same proportion
        int healthIncrease = newMaxHealth - maxHealth;
        currentHealth += healthIncrease;
        maxHealth = newMaxHealth;
    }

    public void IncreaseFlatMaxHealth(int health)
    {
        int newMaxHealth = maxHealth + health;
        // Increase current health by the same proportion
        int healthIncrease = newMaxHealth - maxHealth;
        currentHealth += healthIncrease;
        maxHealth = newMaxHealth;
    }
}