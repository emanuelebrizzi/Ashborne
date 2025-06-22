using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] UIManager UIManager;
    [SerializeField] EnemySpawnManager enemySpawnManager;

    // TODO: move it in a SceneManager componenet
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
        Paused
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            ChangeGameState(GameState.MainMenu);
        }
        else if (scene.name == mainSceneName)
        {
            ChangeGameState(GameState.Playing);

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

            // TODO: Remove when refactoring the campfire
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true;
        }
        else if (CurrentGameState == GameState.Paused && Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeGameState(GameState.Playing);
        }
    }

    // Probably we can extract methods from this method or use switch case statements
    public void ChangeGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        if (newState == GameState.Paused)
        {
            Time.timeScale = 0f;
            UIManager.ShowPauseMenu();
        }
        else if (newState == GameState.Playing)
        {
            Time.timeScale = 1f;
            UIManager.ShowGameplayUI();
            enemySpawnManager.SpawnAllWaves();
        }

        CurrentGameState = newState;
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

    public void RegisterEnemySpawnManager(EnemySpawnManager manager)
    {
        enemySpawnManager = manager;
    }

    public void RegisterUIManager(UIManager manager)
    {
        UIManager = manager;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
