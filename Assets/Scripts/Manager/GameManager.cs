using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] PlayerCommandManager playerCommandManager;
    [SerializeField] UIManager UIManager;
    [SerializeField] EnemySpawnManager enemySpawnManager;
    [SerializeField] AudioManager audioManager;

    [Header("Scene settings")]
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string mainSceneName = "LevelOne";
    [SerializeField] string loadingSceneName = "LoadingScene";
    [SerializeField] float minimumLoadingTime = 0.5f;

    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Campfire,
        SkillTree
    }

    public GameState CurrentGameState { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded; // This is a built-in Unity event
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // TODO: Move this function in another class
        TogglePauseMenu();
    }

    void TogglePauseMenu()
    {
        if (CurrentGameState == GameState.Playing && Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeGameState(GameState.Paused);
        }
        else if (CurrentGameState == GameState.Paused && Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeGameState(GameState.Playing);
        }
    }

    public void ChangeGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        switch (newState)
        {
            case GameState.Paused:
                PauseGame();
                UIManager.ShowPauseMenu();
                break;

            case GameState.Campfire:
                PauseGame();
                UIManager.ShowCampfireMenu();
                break;

            case GameState.Playing:
                UnpauseGame();
                UIManager.ShowGameplayUI();
                break;

            case GameState.SkillTree:
                PauseGame();
                UIManager.ShowSkillTreeMenu();
                break;

            default:
                Debug.LogWarning($"Unhandled game state: {newState}");
                break;
        }

        CurrentGameState = newState;
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void UnpauseGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartNewGame()
    {
        LoadGameScene(mainSceneName);
        Debug.Log("Game Started");
    }

    public void ReturnToMainMenu()
    {
        LoadGameScene(mainMenuSceneName);
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

    public void ResumeGame()
    {
        ChangeGameState(GameState.Playing);
        Debug.Log("Game Resumed");
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

    public void OpenCampfire()
    {
        ChangeGameState(GameState.Campfire);
    }

    public void OpenSkillTree()
    {
        ChangeGameState(GameState.SkillTree);
    }

    public void Rest()
    {
        Player.Instance.Heal();
        enemySpawnManager.SpawnAllWaves();
    }

    public void RegisterEnemySpawnManager(EnemySpawnManager manager)
    {
        enemySpawnManager = manager;
    }

    public void RegisterUIManager(UIManager manager)
    {
        UIManager = manager;
    }

    public void RegisterAudioManager(AudioManager manager)
    {
        audioManager = manager;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            ChangeGameState(GameState.MainMenu);
            if (audioManager != null) audioManager.PlayMenuMusic();
        }
        else if (scene.name == mainSceneName)
        {
            ChangeGameState(GameState.Playing);
            if (audioManager != null) audioManager.PlayGameMusic();
            if (enemySpawnManager != null)
            {
                enemySpawnManager.SpawnAllWaves();
            }
            else
            {
                Debug.LogWarning("EnemySpawnManager not initialized when trying to spawn enemies.");
            }
        }
    }

}
