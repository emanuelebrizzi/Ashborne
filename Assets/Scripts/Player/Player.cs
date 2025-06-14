using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // This class follow the Singleton Pattern
    [SerializeField] SPUM_Prefabs spumPrefabs;
    Health health;
    AshEchoes ashEchoes;

    public static Player Instance { get; private set; }
    public event Action<int> OnHealthChanged;
    public Health Health => health;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Optional: uncomment if you want the player to persist between scenes
            // Maybe it's usefull later 
            // DontDestroyOnLoad(gameObject);
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
        OnHealthChanged?.Invoke(health.CurrentHealth);
    }

    public void AddAshEchoes(int amount)
    {
        if (ashEchoes != null)
        {
            ashEchoes.AddEchoes(amount);
        }
    }

    public bool SpendAshEchoes(int amount)
    {
        if (ashEchoes != null)
        {
            return ashEchoes.SpendEchoes(amount);
        }
        return false;
    }

    public int GetCurrentAshEchoes()
    {
        if (ashEchoes != null)
        {
            return ashEchoes.CurrentAshEchoes;
        }
        return 0;
    }
}
