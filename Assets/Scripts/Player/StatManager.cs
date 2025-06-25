using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Header("Player ")]
    [SerializeField] Health health;
    [SerializeField] Attack attack;
    [SerializeField] PlayerMovement movement;

    [Header("Character Level Stats")]
    [SerializeField] CharacterLevelStats characterStats;

    [Header("Player Multiplicator Stats per Stat Level")]
    [SerializeField] int healthMultiplier = 50;
    [SerializeField] float speedMultiplier = 0.5f;
    [SerializeField] int strengthMultiplier = 2;
    [SerializeField] float attackRangeMultiplier = 0.2f;
    [SerializeField] int energyBallDamageMultiplier = 1;
    [SerializeField] float energyBallRangeMultiplier = 0.5f;

    public static StatsManager Instance { get; private set; }


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

    void Start()
    {
        characterStats.OnStatChanged += UpdateStat;
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
            case StatType.EnergyBall:
                UnlockEnergyBall();
                break;
            case StatType.EnergyBallDamage:
                UpdateEnergyBallDamage();
                break;
            case StatType.EnergyBallRange:
                UpdateEnergyBallRange();
                break;
        }
    }

    void UpdateHealth()
    {
        int statLevel = characterStats.GetStat(StatType.Health);
        health.IncreaseMaxHealth(Mathf.RoundToInt(statLevel * healthMultiplier));
    }

    void UpdateSpeed()
    {
        int statLevel = characterStats.GetStat(StatType.Speed);
        movement.IncreaseSpeed(statLevel * speedMultiplier);
    }

    void UpdateStrength()
    {
        int statLevel = characterStats.GetStat(StatType.Strength);
        attack.IncreaseAttackDamage(Mathf.RoundToInt(statLevel * strengthMultiplier));
    }

    void UpdateAttackRange()
    {
        int statLevel = characterStats.GetStat(StatType.AttackRange);
        attack.IncreaseAttackRange(statLevel * attackRangeMultiplier);
    }


    void UnlockEnergyBall()
    {
        Player.Instance.RangedAttack.enabled = true;
    }

    void UpdateEnergyBallDamage()
    {
        int statLevel = characterStats.GetStat(StatType.EnergyBallDamage);
        Player.Instance.RangedAttack.IncreaseAttackDamage(Mathf.RoundToInt(statLevel * energyBallDamageMultiplier));
    }

    void UpdateEnergyBallRange()
    {
        int statLevel = characterStats.GetStat(StatType.EnergyBallRange);
        Player.Instance.RangedAttack.IncreaseAttackRange(statLevel * energyBallRangeMultiplier);
    }
}

