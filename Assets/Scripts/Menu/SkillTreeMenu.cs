using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SkillTreeMenu : Panel
{
    [SerializeField] SkillTree skillTree;
    [SerializeField] CharacterLevelStats characterStats;

    [Header("Buttons")]
    [SerializeField] Button healthButton;
    [SerializeField] Button speedButton;
    [SerializeField] Button strengthButton;
    [SerializeField] Button attackRangeButton;
    [SerializeField] Button fireballDamageButton;
    [SerializeField] Button fireballRangeButton;
    [SerializeField] Button closeButton;

    [Header("UI Text Elements")]
    [SerializeField] TMPro.TextMeshProUGUI healthText;
    [SerializeField] TMPro.TextMeshProUGUI speedText;
    [SerializeField] TMPro.TextMeshProUGUI strengthText;
    [SerializeField] TMPro.TextMeshProUGUI attackRangeText;
    [SerializeField] TMPro.TextMeshProUGUI fireballDamageText;
    [SerializeField] TMPro.TextMeshProUGUI fireballRangeText;

    private void Start()
    {
        SetupListeners();
        UpdateAllText();
        characterStats.OnStatChanged += UpdateText;
    }
    protected override void SetupListeners()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());
        if (healthButton != null)
            healthButton.onClick.AddListener(() => skillTree.AquireStat(StatType.Health));
        if (speedButton != null)
            speedButton.onClick.AddListener(() => skillTree.AquireStat(StatType.Speed));
        if (strengthButton != null)
            strengthButton.onClick.AddListener(() => skillTree.AquireStat(StatType.Strength));
        if (attackRangeButton != null)
            attackRangeButton.onClick.AddListener(() => skillTree.AquireStat(StatType.AttackRange));
        if (fireballDamageButton != null)
            fireballDamageButton.onClick.AddListener(() => skillTree.AquireStat(StatType.FireballDamage));
        if (fireballRangeButton != null)
            fireballRangeButton.onClick.AddListener(() => skillTree.AquireStat(StatType.FireballRange));
    }

    void UpdateAllText()
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            UpdateText(statType);
        }
    }

    void UpdateText(StatType statType)
    {
        string text = $"{statType}: {characterStats.GetStat(statType)}/{characterStats.GetMaxStat(statType)}\nCost: {skillTree.GetSkillCost(statType)} Echoes";
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