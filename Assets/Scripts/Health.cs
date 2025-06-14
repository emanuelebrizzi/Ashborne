using UnityEngine;

public class Health : MonoBehaviour
{
    /* 
        The current flow updates the health and notifies the changes to the UI using events.
        This class must be adapted to our use case.
    */
    [SerializeField] int currentHealth = 0;
    [SerializeField] int maxHealth = 100;
    public int MaxHealth => maxHealth;

    public delegate void HealthChangedHandler(int currentHealth);
    public event HealthChangedHandler OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Remaining HP: {currentHealth}");
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
}