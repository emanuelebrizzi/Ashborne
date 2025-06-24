using UnityEngine;
using UnityEngine.UI;
using System;

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
    [SerializeField] Button fireballAcquireButton;
    [SerializeField] Button backButton;

    [Header("UI Text Elements")]
    [SerializeField] TMPro.TextMeshProUGUI healthText;
    [SerializeField] TMPro.TextMeshProUGUI speedText;
    [SerializeField] TMPro.TextMeshProUGUI strengthText;
    [SerializeField] TMPro.TextMeshProUGUI attackRangeText;
    [SerializeField] TMPro.TextMeshProUGUI fireballDamageText;
    [SerializeField] TMPro.TextMeshProUGUI fireballRangeText;
    [SerializeField] TMPro.TextMeshProUGUI fireballAcquireText;


    private void Start()
    {
        SetupListeners();
        UpdateAllText();
        characterStats.OnStatChanged += UpdateText;
        fireballAcquireText.text = $"Aquire Fireball\nCost: {skillTree.GetFireballCost()} Echoes";
    }
    protected override void SetupListeners()
    {
        if (backButton != null)
            backButton.onClick.AddListener(() => GameManager.Instance.OpenCampfire());
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
        if (fireballRangeButton != null)
            fireballAcquireButton.onClick.AddListener(() => skillTree.AquireFireball());
    }

    void UpdateAllText()
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            if (statType != StatType.Fireball)
                UpdateText(statType);
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
            case StatType.Fireball:
                FireballAcquired();
                break;
        }
    }
    void FireballAcquired()
    {
        fireballAcquireButton.gameObject.SetActive(false);
        fireballAcquireText.gameObject.SetActive(false);
        fireballDamageText.gameObject.SetActive(true);
        fireballDamageButton.gameObject.SetActive(true);
        fireballRangeButton.gameObject.SetActive(true);
        fireballRangeText.gameObject.SetActive(true);
    }

}