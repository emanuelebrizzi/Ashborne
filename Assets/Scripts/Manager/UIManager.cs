using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameplayPanel;

    [Header("HUD Elements")]
    public Slider healthBar;

    HealthTest health;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        health = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthTest>();
        if (health != null)
        {
            health.OnHealthChanged += UpdateHealthBar;
            Debug.Log("Subscribed to OnHealthChanged event.");
            UpdateHealthBar(100);
        }
        else
        {
            Debug.LogWarning("HealthTest component not found on Player.");
        }
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
            // Normalize the health value before updating the slider
            healthBar.value = (float)hp / health.maxHealth;
            Debug.Log($"Updating health bar: Current Health = {hp}, Max Health = {health.maxHealth}");
        }
        else
        {
            Debug.LogWarning("HealthBar or Health reference is null!");
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
