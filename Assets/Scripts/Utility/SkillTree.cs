using UnityEngine;
using System;

public class SkillTree : MonoBehaviour
{
    [SerializeField] CharacterLevelStats characterStats;
    [SerializeField] Player player;
    [SerializeField] int fireballCost = 200;

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
    public void AquireFireball()
    {
        if (fireballCost <= player.GetEchoes() && !characterStats.IsMaxed(StatType.Fireball))
        {
            if (characterStats.IncreaseStat(StatType.Fireball, 1))
            {
                player.RemoveEchoes(fireballCost);
            }
        }
    }

    public int GetSkillCost(StatType statType)
    {
        return (characterStats.GetStat(statType) + 1) * 100;
    }
    public int GetFireballCost()
    {
        return fireballCost;
    }
    public bool IsBuyable(StatType statType) => (GetSkillCost(statType) <= player.GetEchoes() && !characterStats.IsMaxed(statType));
}
