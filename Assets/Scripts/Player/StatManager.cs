using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    [Header("Character Level Stats")]
    [SerializeField] private CharacterLevelStats characterStats;

    [Header("Player Base Stats")]
    static readonly int HEALTH_BASE = 100;
    static readonly float SPEED_BASE = 5f;
    static readonly int STRENGTH_BASE = 3;
    static readonly float ATTACK_RANGE_BASE = 1f;
    static readonly int FIREBALL_DAMAGE_BASE = 3;
    static readonly float FIREBALL_RANGE_BASE = 5f;

    [Header("Player Multiplicator Stats per Stat Level")]
    static readonly int HEALTH_MULT = 100;
    static readonly float SPEED_MULT = 0.5f;
    static readonly int STRENGTH_MULT = 2;
    static readonly float ATTACK_RANGE_MULT = 0.2f;
    static readonly int FIREBALL_DAMAGE_MULT = 1;
    static readonly float FIREBALL_RANGE_MULT = 0.5f;

    private void Awake()
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

    public int GetHealth() => HEALTH_BASE + characterStats.GetStat(StatType.Health) * HEALTH_MULT;
    public float GetSpeed() => SPEED_BASE + characterStats.GetStat(StatType.Speed) * SPEED_MULT;
    public int GetAttackDamage() => STRENGTH_BASE + characterStats.GetStat(StatType.Strength) * STRENGTH_MULT;
    public float GetAttackRange() => ATTACK_RANGE_BASE + characterStats.GetStat(StatType.AttackRange) * ATTACK_RANGE_MULT;
    public int GetFireballDamage() => FIREBALL_DAMAGE_BASE + characterStats.GetStat(StatType.FireballDamage) * FIREBALL_DAMAGE_MULT;
    public float GetFireballRange() => FIREBALL_RANGE_BASE + characterStats.GetStat(StatType.FireballRange) * FIREBALL_RANGE_MULT;
}
