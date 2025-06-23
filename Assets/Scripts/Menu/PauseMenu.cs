using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Panel
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button exitButton;

    private void Start()
    {
        SetupListeners();
    }

    protected override void SetupListeners()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());

        if (menuButton != null)
            menuButton.onClick.AddListener(() => GameManager.Instance.ReturnToMainMenu());

        if (exitButton != null)
            exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
    }
}
