using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string loadingSceneName = "LoadingScene";
    [SerializeField] private float minimumLoadingTime = 0.5f;

    public static GameManager Instance { get; private set; }

    public UIManager UIManager { get; private set; }

    public enum GameState
    {
        Playing,
        Paused
    }

    public GameState CurrentGameState { get; private set; } = GameState.Playing;

    public delegate void GameStateChangedHandler(GameState newState);
    public event GameStateChangedHandler OnGameStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        UIManager = GetComponentInChildren<UIManager>();
        DontDestroyOnLoad(gameObject);
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

    public void StartNewGame()
    {
        ChangeGameState(GameState.Playing);
        LoadGameScene("SampleScene");
        Debug.Log("Game Started");
    }


    public void LoadGameScene(string targetSceneName)
    {
        StartCoroutine(LoadSceneWithTransition(targetSceneName));
    }

    IEnumerator LoadSceneWithTransition(string targetSceneName)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(loadingSceneName);
        while (!loadingOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(minimumLoadingTime);

        SceneManager.LoadScene(targetSceneName);
    }

    public void PauseGame()
    {
        ChangeGameState(GameState.Paused);
        Debug.Log("Game Paused");
    }

    public void QuitGame()
    {
        Debug.Log("Exiting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
