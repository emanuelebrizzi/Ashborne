using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] SPUM_Prefabs spumPrefabs;
    Health health;
    Attack meleeAttack;
    Attack rangedAttack;
    AshEchoes ashEchoes;
    [SerializeField] PlayerDeathHandler playerDeathHandler;

    public Attack MeleeAttack => meleeAttack;
    public Attack RangedAttack => rangedAttack;

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

        health.OnDeath += Die;
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

        meleeAttack = GetComponent<MeleeAttack>();
        rangedAttack = GetComponent<RangedAttack>();
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
        health.ApplyDamaage(damage);
        var newValue = (float)health.CurrentHealth / health.MaxHealth;
        OnHealthChanged?.Invoke(newValue);
        PlayAnimation(PlayerState.DAMAGED, 0);
    }

    void Die()
    {
        PlayAnimation(PlayerState.DEATH, 0);
        if (playerDeathHandler != null)
        {
            playerDeathHandler.Die();
        }

        OnEchoesChanged?.Invoke(ashEchoes.Current);
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

    public void Heal()
    {
        health.ResetHealth();
        OnHealthChanged?.Invoke(health.CurrentHealth);
    }

    public AshEchoes AshEchoes { get { return ashEchoes; } }
    public Health Health { get { return health; } }

}
