using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterLevelStats : MonoBehaviour
{
    private struct LevelStat
    {
        public int value;
        public int maxValue;

        public LevelStat(int initialValue, int maxValue)
        {
            this.value = initialValue;
            this.maxValue = maxValue;
        }

        public bool Increase(int amount)
        {
            if (value + amount <= maxValue)
            {
                value += amount;
                return true;
            }
            return false;
        }

        public readonly bool IsMaxed() => value >= maxValue;
    }

    private Dictionary<StatType, LevelStat> stats;
    public event Action<StatType> OnStatChanged;

    private void Awake()
    {
        stats = new Dictionary<StatType, LevelStat>
        {
            { StatType.Health,         new LevelStat(0, 5) },
            { StatType.Speed,          new LevelStat(0, 3) },
            { StatType.Strength,       new LevelStat(0, 5) },
            { StatType.AttackRange,    new LevelStat(0, 2) },
            { StatType.Fireball,       new LevelStat(0, 1) },
            { StatType.FireballDamage, new LevelStat(0, 5) },
            { StatType.FireballRange,  new LevelStat(0, 2) }
        };
    }

    public bool IncreaseStat(StatType statType, int amount)
    {
        if (stats.TryGetValue(statType, out LevelStat stat))
        {
            if (stat.Increase(amount))
            {
                stats[statType] = stat;
                OnStatChanged?.Invoke(statType);
                Debug.Log($"Payler {statType} {stat.value}");
                return true;
            }
        }
        return false;
    }

    public int GetStat(StatType statType)
    {
        return stats.TryGetValue(statType, out LevelStat stat) ? stat.value : 0;
    }

    public int GetMaxStat(StatType statType)
    {
        return stats.TryGetValue(statType, out LevelStat stat) ? stat.maxValue : 0;
    }

    public bool IsMaxed(StatType statType)
    {
        return stats.TryGetValue(statType, out LevelStat stat) && stat.IsMaxed();
    }
}
