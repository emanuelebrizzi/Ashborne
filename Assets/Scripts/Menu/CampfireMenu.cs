using UnityEngine;
using UnityEngine.UI;

public class CampfireMenu : Panel
{
    [SerializeField] Button skillTreeButton;
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
        if (skillTreeButton != null)
            skillTreeButton.onClick.AddListener(() => GameManager.Instance.OpenSkillTree());

        if (restButton != null)
            restButton.onClick.AddListener(() => GameManager.Instance.Rest());

        if (exitButton != null)
            exitButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());
    }
}




