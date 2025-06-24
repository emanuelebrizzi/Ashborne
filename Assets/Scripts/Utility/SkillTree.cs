using UnityEngine;
using System;

public class SkillTree : MonoBehaviour
{
    [SerializeField] CharacterLevelStats characterStats;
    [SerializeField] Player player;
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

    public int GetSkillCost(StatType statType)
    {
        return (characterStats.GetStat(statType) + 1) * 100;
    }
    public bool IsBuyable(StatType statType) => (GetSkillCost(statType) <= player.GetEchoes() && !characterStats.IsMaxed(statType));
}
