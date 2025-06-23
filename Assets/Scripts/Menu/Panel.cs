using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    [SerializeField] protected GameObject panel;

    public void Show()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    protected virtual void SetupListeners() { }
}
