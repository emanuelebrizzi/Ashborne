using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Header("Player ")]
    [SerializeField] PlayerMovement movement;

    [Header("Character Level Stats")]
    [SerializeField] CharacterLevelStats characterStats;

    [Header("Player Multiplicator Stats per Stat Level")]
    [SerializeField] int healthMultiplier = 50;
    [SerializeField] float speedMultiplier = 0.5f;
    [SerializeField] int strengthMultiplier = 2;
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
        Player.Instance.Health.IncreaseMaxHealth(Mathf.RoundToInt(statLevel * healthMultiplier));
    }

    void UpdateSpeed()
    {
        int statLevel = characterStats.GetStat(StatType.Speed);
        movement.IncreaseSpeed(statLevel * speedMultiplier);
    }

    void UpdateStrength()
    {
        int statLevel = characterStats.GetStat(StatType.Strength);
        Player.Instance.MeleeAttack.IncreaseAttackDamage(Mathf.RoundToInt(statLevel * strengthMultiplier));
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

