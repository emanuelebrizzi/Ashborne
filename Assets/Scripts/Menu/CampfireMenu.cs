using UnityEngine;
using UnityEngine.UI;

public class CampfireMenu : Panel
{
    [SerializeField] Button statTreeButton;
    [SerializeField] Button restButton;
    [SerializeField] Button exitButton;

    private void Start()
    {
        if (exitButton != null)
            exitButton.onClick.RemoveAllListeners();

        SetupListeners();
    }

    protected override void SetupListeners()
    {
        if (statTreeButton != null)
            statTreeButton.onClick.AddListener(() => GameManager.Instance.OpenStatTree());

        if (restButton != null)
            restButton.onClick.AddListener(() => GameManager.Instance.Rest());

        if (exitButton != null)
            exitButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());
    }
}




