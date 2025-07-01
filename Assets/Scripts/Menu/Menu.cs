using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    /*  This abstraction is used to improve the management of GUI elements for the UIManager. 
        New menu types can be added without recompiling the UIManager. Each concrete class will 
        handle different UI elements, such as buttons and texts.
    */

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
