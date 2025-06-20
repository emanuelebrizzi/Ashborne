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


    void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterUIManager(this);
    }

    void Start()
    {
        RegisterToEventHandlers();
        InitializeUIElements();
        SetUpMainMenuListeners();
        SetUpPauseMenuListeners();
    }

    void RegisterToEventHandlers()
    {
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
    }


    void SetUpMainMenuListeners()
    {
        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(() => GameManager.Instance.StartNewGame());
        }

        // TODO: implement when there is the loading state
        // if (loadGameButton != null)
        // {
        // }

        if (exitMainMenuButton != null)
        {
            exitMainMenuButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        }
    }

    void SetUpPauseMenuListeners()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(() => GameManager.Instance.ReturnToMainMenu());
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
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

    public void ShowPauseMenu()
    {
        if (gameplayPanel) gameplayPanel.SetActive(true);
        if (pauseMenuPanel) pauseMenuPanel.SetActive(true);
    }

    public void ShowGameplayUI()
    {
        if (gameplayPanel) gameplayPanel.SetActive(true);
        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
    }

    void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged -= UpdateHealthBar;
            Player.Instance.OnEchoesChanged -= UpdateAshEchoes;
        }
    }
}
