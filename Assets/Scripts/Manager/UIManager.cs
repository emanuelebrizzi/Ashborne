using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    const int InitialHealthBarValue = 1;
    const int InitialEchoesValue = 0;

    [Header("UI Panels")]
    [SerializeField] GameObject gameplayPanel;
    [SerializeField] GameObject pauseMenuPanel;

    [Header("HUD Elements")]
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI ashEchoesText;


    void Start()
    {
        RegisterToEventHandlers();
        InitializeUIElements();
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

    void UpdateUIPanels(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.Playing:
                if (gameplayPanel) gameplayPanel.SetActive(true);
                if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
                break;
            case GameManager.GameState.Paused:
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
