using UnityEngine;
using System;

public class StatTree : MonoBehaviour
{
    [SerializeField] CharacterLevelStats characterStats;
    [SerializeField] int energyBallCost = 200;

    public void AquireStat(StatType statType)
    {
        if (IsBuyable(statType))
        {
            int cost = GetStatCost(statType);
            if (characterStats.IncreaseStat(statType, 1))
            {
                Player.Instance.RemoveEchoes(cost);
            }
        }
    }

    public void AquireEnergyBall()
    {
        if (energyBallCost <= Player.Instance.GetEchoes() && !characterStats.IsMaxed(StatType.EnergyBall))
        {
            if (characterStats.IncreaseStat(StatType.EnergyBall, 1))
            {
                Player.Instance.RemoveEchoes(energyBallCost);
            }
        }
    }

    public int GetStatCost(StatType statType)
    {
        return (characterStats.GetStat(statType) + 1) * 100;
    }

    public int GetEnergyBallCost()
    {
        return energyBallCost;
    }

    public bool IsBuyable(StatType statType) => GetStatCost(statType) <= Player.Instance.GetEchoes() && !characterStats.IsMaxed(statType);
}
