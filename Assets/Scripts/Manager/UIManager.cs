using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    Health health;
    AshEchoes ashEchoes;

    [Header("UI Panels")]
    public GameObject gameplayPanel;

    [Header("HUD Elements")]
    public Slider healthBar;
    public TextMeshProUGUI ashEchoesText;



    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        if (health != null)
        {
            health.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(health.MaxHealth);
            Debug.Log("Subscribed to OnHealthChanged event.");
        }

        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<AshEchoes>(out var ashEchoes))
        {
            ashEchoes.OnEchoesChanged += UpdateAshEchoes;
            UpdateAshEchoes(ashEchoes.CurrentAshEchoes);
            Debug.Log("Subscribed to OnEchoesChanged event.");
        }

        UpdateAshEchoes(0);
    }


    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        if (health != null)
        {
            health.OnHealthChanged -= UpdateHealthBar;
        }

        if (ashEchoes != null)
        {
            ashEchoes.OnEchoesChanged -= UpdateAshEchoes;
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

        }
    }

    private void HideAllPanels()
    {
        if (gameplayPanel) gameplayPanel.SetActive(false);
    }

    private void ShowPanel(GameObject panel)
    {
        if (panel) panel.SetActive(true);
    }

    public void UpdateHealthBar(int hp)
    {
        if (healthBar != null && health != null)
        {
            healthBar.value = (float)hp / health.MaxHealth;
            Debug.Log($"Updating health bar: Current Health = {hp}, Max Health = {health.MaxHealth}");
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

    public void UpdateAshEchoes(int amount)
    {
        if (ashEchoesText != null)
        {
            ashEchoesText.text = amount.ToString();
        }
    }
}
