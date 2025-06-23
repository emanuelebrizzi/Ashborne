using UnityEngine;
using System;

public class SkillTreeMenu : MonoBehaviour
{
    [SerializeField] GameObject skillTreePanel;
    [SerializeField] CharacterLevelStats characterStats;
    [SerializeField] Player player;

    [Header("UI Text Elements")]
    [SerializeField] TMPro.TextMeshProUGUI healthText;
    [SerializeField] TMPro.TextMeshProUGUI speedText;
    [SerializeField] TMPro.TextMeshProUGUI strengthText;
    [SerializeField] TMPro.TextMeshProUGUI attackRangeText;
    [SerializeField] TMPro.TextMeshProUGUI fireballDamageText;
    [SerializeField] TMPro.TextMeshProUGUI fireballRangeText;


    private void Start()
    {
        if (skillTreePanel != null)
        {
            skillTreePanel.SetActive(false);
        }
        UpdateAllText();
        characterStats.OnStatChanged += UpdateText;
    }


    public void OpenSkillTree()
    {
        if (skillTreePanel != null)
        {
            skillTreePanel.SetActive(true);
            Time.timeScale = 0; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible
        }
    }
    public void CloseSkillTree()
    {
        if (skillTreePanel != null)
        {
            skillTreePanel.SetActive(false);
            Time.timeScale = 1; // Resume the game
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Hide the cursor
        }
    }

    public void AquireStat(StatType statType)
    {
        if (IsBuyable(statType))
        {
            if (characterStats.IncreaseStat(statType, 1))
            {
                player.RemoveEchoes(GetSkillCost(statType));
            }
        }
    }


    public void AquireStat(string statType)
    {
        Enum.TryParse(statType, out StatType parsedStatType);
        AquireStat(parsedStatType);
    }


    public int GetSkillCost(StatType statType)
    {
        return characterStats.GetStat(statType) + 1 * 100;
    }

    public bool IsBuyable(StatType statType) => (GetSkillCost(statType) <= player.GetEchoes() || !characterStats.IsMaxed(statType));

    private void UpdateAllText()
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            UpdateText(statType);
        }
    }
    private void UpdateText(StatType statType)
    {
        string text = $"{statType}: {characterStats.GetStat(statType)}/{characterStats.GetMaxStat(statType)}\nCost: {GetSkillCost(statType)} Echoes";
        switch (statType)
        {
            case StatType.Health:
                healthText.text = text;
                break;
            case StatType.Speed:
                speedText.text = text;
                break;
            case StatType.Strength:
                strengthText.text = text;
                break;
            case StatType.AttackRange:
                attackRangeText.text = text;
                break;
            case StatType.FireballDamage:
                fireballDamageText.text = text;
                break;
            case StatType.FireballRange:
                fireballRangeText.text = text;
                break;
        }
    }

}
