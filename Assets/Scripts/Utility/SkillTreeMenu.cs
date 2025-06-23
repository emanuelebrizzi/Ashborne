using UnityEngine;

public class SkillTreeMenu : MonoBehaviour
{
    [SerializeField] private GameObject skillTreePanel;
    [SerializeField] private CharacterLevelStats characterStats;
    [SerializeField] private Player player;

    private void Start()
    {
        if (skillTreePanel != null)
        {
            skillTreePanel.SetActive(false); // Hide the skill tree panel at the start
        }
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

    public void IncreaseStat(StatType statType)
    {
        if (IsBuyable(statType))
        {
            if (characterStats.IncreaseStat(statType, 1))
            {
                player.RemoveEchoes(GetSkillCost(statType));
            }
        }
    }

    public int GetSkillCost(StatType statType)
    {
        return characterStats.GetStat(statType) + 1 * 100;
    }

    public bool IsBuyable(StatType statType) => (GetSkillCost(statType) <= player.GetEchoes() || !characterStats.IsMaxed(statType));
}
