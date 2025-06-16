using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UIManager UIManager { get; private set; }

    public enum GameState
    {
        Playing,
        Paused
    }

    public GameState CurrentGameState { get; private set; } = GameState.Playing;

    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        UIManager = GetComponentInChildren<UIManager>();
    }

    public void ChangeGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);

        // Setting timeScale to 0 will freeze updates
        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }
    }

    public void StartGame()
    {
        ChangeGameState(GameState.Playing);
        Debug.Log("Game Started");
    }

    public void PauseGame()
    {
        ChangeGameState(GameState.Paused);
        Debug.Log("Game Paused");
    }
}
