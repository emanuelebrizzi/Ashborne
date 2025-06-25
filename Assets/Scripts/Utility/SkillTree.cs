using UnityEngine;
using System;

public class SkillTree : MonoBehaviour
{
    [SerializeField] CharacterLevelStats characterStats;
    [SerializeField] Player player;
    [SerializeField] int energyBallCost = 200;

    public void AquireStat(StatType statType)
    {
        if (IsBuyable(statType))
        {
            int cost = GetSkillCost(statType);
            if (characterStats.IncreaseStat(statType, 1))
            {
                player.RemoveEchoes(cost);
            }
        }
    }

    public void AquireEnergyBall()
    {
        if (energyBallCost <= player.GetEchoes() && !characterStats.IsMaxed(StatType.EnergyBall))
        {
            if (characterStats.IncreaseStat(StatType.EnergyBall, 1))
            {
                player.RemoveEchoes(energyBallCost);
            }
        }
    }

    public int GetSkillCost(StatType statType)
    {
        return (characterStats.GetStat(statType) + 1) * 100;
    }

    public int GetEnergyBallCost()
    {
        return energyBallCost;
    }

    public bool IsBuyable(StatType statType) => GetSkillCost(statType) <= player.GetEchoes() && !characterStats.IsMaxed(statType);
}
