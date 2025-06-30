using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] UIManager UIManager;
    [SerializeField] EnemySpawnManager enemySpawnManager;
    [SerializeField] AudioManager audioManager;

    [Header("Scene settings")]
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string mainSceneName = "LevelOne";
    [SerializeField] string loadingSceneName = "LoadingScene";
    [SerializeField] float minimumLoadingTime = 0.5f;

    GameState currentState;

    public static GameManager Instance { get; private set; }

    /*  We use these game states to handle differnet scenarios of our game. For example, during  
        StatTree we want to give the player the ability to acquire power ups meanwhile the game is
        paused.
    */
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Campfire,
        StatTree
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    public void TogglePauseMenu()
    {
        if (currentState == GameState.Playing)
        {
            ChangeGameState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            ChangeGameState(GameState.Playing);
        }
    }

    public void ChangeGameState(GameState newState)
    {
        if (currentState == newState) return;

        switch (newState)
        {
            case GameState.Paused:
                PauseGame();
                UIManager.ShowMenu<PauseMenu>();
                break;

            case GameState.Campfire:
                PauseGame();
                UIManager.ShowMenu<CampfireMenu>();
                break;

            case GameState.Playing:
                UnpauseGame();
                UIManager.HideAllMenus();
                UIManager.EnableHUD();
                break;

            case GameState.StatTree:
                PauseGame();
                UIManager.ShowMenu<StatTreeMenu>();
                break;

            default:
                Debug.LogWarning($"Unhandled game state: {newState}");
                break;
        }

        currentState = newState;
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

    public void OpenStatTree()
    {
        ChangeGameState(GameState.StatTree);
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
        // We are loading the scene with the default SINGLE mode.
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
        }
    }

}
