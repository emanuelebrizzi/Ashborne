using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // This class follow the Singleton Pattern
    [SerializeField] SPUM_Prefabs spumPrefabs;
    Health health;
    AshEchoes ashEchoes;
    [SerializeField] PlayerDeathHandler playerDeathHandler;
    public static Player Instance { get; private set; }
    public event Action<float> OnHealthChanged;
    public event Action<int> OnEchoesChanged;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        health = GetComponent<Health>();
        ashEchoes = GetComponent<AshEchoes>();
    }
    void Start()
    {
        spumPrefabs = GetComponent<SPUM_Prefabs>();
        if (spumPrefabs != null)
        {
            if (!spumPrefabs.allListsHaveItemsExist())
            {
                spumPrefabs.PopulateAnimationLists();
            }

            spumPrefabs.OverrideControllerInit();
        }

    }

    public void PlayAnimation(PlayerState state, int index = 0)
    {
        if (spumPrefabs != null)
        {
            spumPrefabs.PlayAnimation(state, index);
        }
    }

    public void TakeDamage(int damage)
    {
        PlayAnimation(PlayerState.DAMAGED, 0);
        health.TakeDamage(damage);
        if (health.CurrentHealth <= 0)
        {
            OnHealthChanged?.Invoke(0);
            Die();
            OnEchoesChanged?.Invoke(ashEchoes.Current);
        }
        var newValue = (float)health.CurrentHealth / health.MaxHealth;
        OnHealthChanged?.Invoke(newValue);

    }

    void Die()
    {
        PlayAnimation(PlayerState.DEATH, 0);

        if (playerDeathHandler != null)
        {

            playerDeathHandler.Die();
            PlayAnimation(PlayerState.MOVE, 0); // Reset to move state after death animation
        }
        else
        {
            Debug.LogError("PlayerDeathHandler is not assigned to the player.");
        }

    }

    public void AddAshEchoes(int amount)
    {
        if (ashEchoes != null)
        {
            ashEchoes.AddEchoes(amount);
        }
        OnEchoesChanged?.Invoke(ashEchoes.Current);
    }

    public void RemoveEchoes(int amount)
    {
        if (ashEchoes != null)
        {
            ashEchoes.RemoveEchoes(amount);
        }
        OnEchoesChanged?.Invoke(ashEchoes.Current);
    }
    public int GetEchoes()
    {
        return ashEchoes.Current;
    }

    public AshEchoes AshEchoes { get { return ashEchoes; } }
    public Health Health { get { return health; } }

}
