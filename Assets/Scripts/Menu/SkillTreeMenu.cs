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
    [SerializeField] Button energyBallDamageButton;
    [SerializeField] Button energyBallRangeButton;
    [SerializeField] Button energyBallAcquireButton;
    [SerializeField] Button backButton;

    [Header("UI Text Elements")]
    [SerializeField] TMPro.TextMeshProUGUI healthText;
    [SerializeField] TMPro.TextMeshProUGUI speedText;
    [SerializeField] TMPro.TextMeshProUGUI strengthText;
    [SerializeField] TMPro.TextMeshProUGUI attackRangeText;
    [SerializeField] TMPro.TextMeshProUGUI enrgyBallDamageText;
    [SerializeField] TMPro.TextMeshProUGUI energyBallRangeText;
    [SerializeField] TMPro.TextMeshProUGUI energyBallAcquireText;


    void Start()
    {
        SetupListeners();
        UpdateAllText();
        characterStats.OnStatChanged += UpdateText;
        energyBallAcquireText.text = $"Aquire EnergyBall\nCost: {skillTree.GetEnergyBallCost()} Echoes";
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
        if (energyBallDamageButton != null)
            energyBallDamageButton.onClick.AddListener(() => skillTree.AquireStat(StatType.EnergyBallDamage));
        if (energyBallRangeButton != null)
            energyBallRangeButton.onClick.AddListener(() => skillTree.AquireStat(StatType.EnergyBallRange));
        if (energyBallRangeButton != null)
            energyBallAcquireButton.onClick.AddListener(() => skillTree.AquireEnergyBall());
    }

    void UpdateAllText()
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            if (statType != StatType.EnergyBall)
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
            case StatType.EnergyBallDamage:
                enrgyBallDamageText.text = text;
                break;
            case StatType.EnergyBallRange:
                energyBallRangeText.text = text;
                break;
            case StatType.EnergyBall:
                ShowEnergyBallUpgrades();
                break;
        }
    }
    void ShowEnergyBallUpgrades()
    {
        energyBallAcquireButton.gameObject.SetActive(false);
        energyBallAcquireText.gameObject.SetActive(false);
        enrgyBallDamageText.gameObject.SetActive(true);
        energyBallDamageButton.gameObject.SetActive(true);
        energyBallRangeButton.gameObject.SetActive(true);
        energyBallRangeText.gameObject.SetActive(true);
    }

}