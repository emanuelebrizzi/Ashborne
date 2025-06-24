using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] MainMenu mainMenu;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] GameplayHUD gameplayHUD;
    [SerializeField] CampfireMenu campfireMenu;
    [SerializeField] SkillTreeMenu skillTreeMenu;

    void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterUIManager(this);
    }

    public void ShowMainMenu()
    {
        if (mainMenu) mainMenu.Show();
        if (pauseMenu) pauseMenu.Hide();
        if (gameplayHUD) gameplayHUD.Hide();
        if (campfireMenu) campfireMenu.Hide();
        if (skillTreeMenu) skillTreeMenu.Hide();
    }

    public void ShowPauseMenu()
    {
        if (mainMenu) mainMenu.Hide();
        if (pauseMenu) pauseMenu.Show();
        if (gameplayHUD) gameplayHUD.Show();
        if (campfireMenu) campfireMenu.Hide();
        if (skillTreeMenu) skillTreeMenu.Hide();
    }

    public void ShowGameplayUI()
    {
        if (mainMenu) mainMenu.Hide();
        if (pauseMenu) pauseMenu.Hide();
        if (gameplayHUD) gameplayHUD.Show();
        if (campfireMenu) campfireMenu.Hide();
        if (skillTreeMenu) skillTreeMenu.Hide();
    }

    public void ShowCampfireMenu()
    {
        if (mainMenu) mainMenu.Hide();
        if (pauseMenu) pauseMenu.Hide();
        if (gameplayHUD) gameplayHUD.Hide();
        if (campfireMenu) campfireMenu.Show();
        if (skillTreeMenu) skillTreeMenu.Hide();

    }
    public void ShowSkillTreeMenu()
    {
        if (mainMenu) mainMenu.Hide();
        if (pauseMenu) pauseMenu.Hide();
        if (gameplayHUD) gameplayHUD.Hide();
        if (campfireMenu) campfireMenu.Hide();
        if (skillTreeMenu) skillTreeMenu.Show();
    }
}
