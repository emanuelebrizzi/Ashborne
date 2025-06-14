using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] GameObject gameplayPanel;

    [Header("HUD Elements")]
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI ashEchoesText;

    AshEchoes ashEchoes;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(Player.Instance.Health.MaxHealth);

        }

        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<AshEchoes>(out var ashEchoesComponent))
        {
            ashEchoes = ashEchoesComponent;
            ashEchoes.OnEchoesChanged += UpdateAshEchoes;
            UpdateAshEchoes(ashEchoes.CurrentAshEchoes);
            Debug.Log("Subscribed to OnEchoesChanged event.");
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
        }

        if (ashEchoes != null)
        {
            ashEchoes.OnEchoesChanged -= UpdateAshEchoes;
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

    public void UpdateHealthBar(int hp)
    {
        if (healthBar != null && Player.Instance.Health != null)
        {
            healthBar.value = (float)hp / Player.Instance.Health.MaxHealth;
            Debug.Log($"Updating health bar: Current Health = {hp}, Max Health = {Player.Instance.Health.MaxHealth}");
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
