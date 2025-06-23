using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] GameplayHUD gameplayHUD;

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
    }

    public void ShowPauseMenu()
    {
        if (mainMenu) mainMenu.Hide();
        if (pauseMenu) pauseMenu.Show();
        if (gameplayHUD) gameplayHUD.Show();
    }

    public void ShowGameplayUI()
    {
        if (mainMenu) mainMenu.Hide();
        if (pauseMenu) pauseMenu.Hide();
        if (gameplayHUD) gameplayHUD.Show();
    }
}
