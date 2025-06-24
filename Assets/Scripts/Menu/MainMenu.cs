using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Panel
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button exitButton;

    private void Start()
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
