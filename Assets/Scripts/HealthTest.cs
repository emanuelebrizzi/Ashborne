using UnityEngine;

public class HealthTest : MonoBehaviour
{
    /* 
        The current flow updates the health and notifies the changes to the UI using events.
    */
    [SerializeField] int curHealth = 0;
    [SerializeField] public int maxHealth = 100;

    public delegate void HealthChangedHandler(int currentHealth);
    public event HealthChangedHandler OnHealthChanged;

    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DamagePlayer(10);
        }
    }
    public void DamagePlayer(int damage)
    {
        curHealth -= damage;
        OnHealthChanged?.Invoke(curHealth);
    }
}