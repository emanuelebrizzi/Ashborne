using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameplayPanel;
    public GameObject pausePanel;

    [Header("HUD Elements")]
    public Slider healthBar;
    public TextMeshProUGUI ashSoulsText;

    int currentAshSouls = 0;
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
            {
                ShowPauseMenu();
            }
            else if (GameManager.Instance.CurrentGameState == GameManager.GameState.Paused)
            {
                ResumeGameplay();
            }
        }
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        HideAllPanels();

        switch (newState)
        {
            case GameManager.GameState.Playing:
                ShowPanel(gameplayPanel);
                break;
            case GameManager.GameState.Paused:
                ShowPanel(pausePanel);
                break;

        }
    }

    private void HideAllPanels()
    {
        if (gameplayPanel) gameplayPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
    }

    private void ShowPanel(GameObject panel)
    {
        if (panel) panel.SetActive(true);
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    public void UpdateAshSouls(int ashSouls)
    {
        currentAshSouls = ashSouls;
        UpdateAshSoulsDisplay();
    }

    void UpdateAshSoulsDisplay()
    {
        if (ashSoulsText != null)
        {
            ashSoulsText.text = "Ash Souls: " + currentAshSouls;
        }
    }

    public void ShowPauseMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.Paused);
        }
    }

    public void ResumeGameplay()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.Playing);
        }
    }
}
