using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    const int InitialHealthBarValue = 1;
    const int InitialEchoesValue = 0;

    [Header("Main Menu Elements")]
    [SerializeField] Button newGameButton;
    [SerializeField] Button loadGameButton;
    [SerializeField] Button exitMainMenuButton;

    [Header("Pause Menu Elements")]
    [SerializeField] GameObject gameplayPanel;
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] Button resumeButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button exitButton;
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI ashEchoesText;

    void Start()
    {
        RegisterToEventHandlers();
        InitializeUIElements();
        SetupButtonListeners();
    }

    void RegisterToEventHandlers()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += UpdateUIPanels;
        }

        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged += UpdateHealthBar;
            Player.Instance.OnEchoesChanged += UpdateAshEchoes;
        }
    }

    void InitializeUIElements()
    {
        UpdateHealthBar(InitialHealthBarValue);
        UpdateAshEchoes(InitialEchoesValue);
        UpdateUIPanels(GameManager.GameState.Playing);
    }

    void SetupButtonListeners()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ResumeGame();
            });
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReturnToMainMenu();
            });
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(() =>
            {

                GameManager.Instance.QuitGame();
            });
        }

        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(() => GameManager.Instance.StartNewGame());
        }

        // TODO:   implement when there is the loading state
        // if (loadGameButton != null)
        // {
        // }

        if (exitMainMenuButton != null)
        {
            exitMainMenuButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        }

    }
    void UpdateUIPanels(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.MainMenu:
                if (gameplayPanel) gameplayPanel.SetActive(false);
                if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
                break;

            case GameManager.GameState.Playing:
                if (gameplayPanel) gameplayPanel.SetActive(true);
                if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
                break;

            case GameManager.GameState.Paused:
                if (gameplayPanel) gameplayPanel.SetActive(true);
                if (pauseMenuPanel) pauseMenuPanel.SetActive(true);
                break;
        }
    }

    public void UpdateHealthBar(float currentHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    public void UpdateAshEchoes(int amount)
    {
        if (ashEchoesText != null)
        {
            ashEchoesText.text = amount.ToString();
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= UpdateUIPanels;
        }

        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged -= UpdateHealthBar;
            Player.Instance.OnEchoesChanged -= UpdateAshEchoes;
        }
    }
}
