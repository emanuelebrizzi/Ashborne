using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button exitButton;

    void Start()
    {
        SetupListeners();
    }

    protected override void SetupListeners()
    {
        if (newGameButton != null)
            newGameButton.onClick.AddListener(() => GameManager.Instance.StartNewGame());

        if (exitButton != null)
            exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
    }
}
