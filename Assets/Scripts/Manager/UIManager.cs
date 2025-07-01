using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] HUD gameplayHUD;
    [SerializeField] Menu[] menus;

    void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterUIManager(this);
    }

    public void EnableHUD()
    {
        if (gameplayHUD) gameplayHUD.gameObject.SetActive(true);
    }

    public void ShowMenu<T>() where T : Menu
    {
        foreach (var menu in menus)
        {
            if (menu is T)
                menu.Show();
            else
                menu.Hide();
        }
    }

    public void HideAllMenus()
    {
        /*  The assumption is that the HUD should always be enabled during the gameplay, so we need
            this helper method for the Game Manager.
        */

        foreach (var menu in menus)
        {
            menu.Hide();
        }
    }
}
