using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    [Header("Player ")]
    [SerializeField] Health health;
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerAttack attack;

    [Header("Character Level Stats")]
    [SerializeField] CharacterLevelStats characterStats;

    [Header("Player Multiplicator Stats per Stat Level")]
    [SerializeField] int healthMultiplier = 50;
    [SerializeField] float speedMultiplier = 0.5f;
    [SerializeField] int strengthMultiplier = 2;
    [SerializeField] float attackRangeMultiplier = 0.2f;
    [SerializeField] int fireballDamageMultiplier = 1;
    [SerializeField] float fireballRangeMultiplier = 0.5f;

    void Start()
    {
        characterStats.OnStatChanged += UpdateStat;
    }
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
    }

 void UpdateStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health:
                UpdateHealth();
                break;
            case StatType.Speed:
                UpdateSpeed();
                break;
            case StatType.Strength:
                UpdateStrength();
                break;
            case StatType.AttackRange:
                UpdateAttackRange();
                break;
            case StatType.FireballDamage:
                UpdateFireballDamage();
                break;
            case StatType.FireballRange:
                UpdateFireballRange();
                break;  
        }
    }

    void UpdateHealth()
    {
        int statLevel = characterStats.GetStat(StatType.Health);
        health.IncreaseFlatMaxHealth(Mathf.RoundToInt(statLevel * healthMultiplier));
    }

    void UpdateSpeed()
    {
        int statLevel = characterStats.GetStat(StatType.Speed);
        movement.IncreaseFlatSpeed(statLevel * speedMultiplier);
    }

    void UpdateStrength()
    {
        int statLevel = characterStats.GetStat(StatType.Strength);
        attack.IncreaseFlatDamage(Mathf.RoundToInt(statLevel * strengthMultiplier));
    }

    void UpdateAttackRange()
    {
        int statLevel = characterStats.GetStat(StatType.AttackRange);
        attack.IncreaseFlatRange(statLevel * attackRangeMultiplier);
    }

    void UpdateFireballDamage()
    {
    // TODO: Implement Fireball first  
    }

    void UpdateFireballRange()
    {
    // TODO: Implement Fireball first   
    }

}

