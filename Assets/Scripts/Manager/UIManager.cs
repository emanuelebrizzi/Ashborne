using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] GameObject gameplayPanel;

    [Header("HUD Elements")]
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI ashEchoesText;


    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(1);

            Player.Instance.OnEchoesChanged += UpdateAshEchoes;
            UpdateAshEchoes(0);
        }
    }


    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged -= UpdateHealthBar;
            Player.Instance.OnEchoesChanged -= UpdateAshEchoes;
        }
    }


    void HandleGameStateChanged(GameManager.GameState newState)
    {
        HideAllPanels();

        switch (newState)
        {
            case GameManager.GameState.Playing:
                ShowPanel(gameplayPanel);
                break;

        }
    }

    void HideAllPanels()
    {
        if (gameplayPanel) gameplayPanel.SetActive(false);
    }

    private void ShowPanel(GameObject panel)
    {
        if (panel) panel.SetActive(true);
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
}
