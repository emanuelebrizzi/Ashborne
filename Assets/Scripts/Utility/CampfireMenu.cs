using UnityEngine;

public class CampfireMenu : MonoBehaviour
{

    [SerializeField] GameObject campfireMenuUI; // Reference to the campfire menu UI
    void Start()
    {
        if (campfireMenuUI != null)
        {
            campfireMenuUI.SetActive(false); // Hide the menu at the start
        }
    }

    public void OpenCampfireMenu()
    {
        if (campfireMenuUI != null)
        {
            campfireMenuUI.SetActive(true); // Show the campfire menu
            Time.timeScale = 0; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible

            // Disable player controls

        }
    }

    public void CloseCampfireMenu()
    {
        if (campfireMenuUI != null)
        {
            campfireMenuUI.SetActive(false); // Hide the campfire menu
            Time.timeScale = 1; // Resume the game
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Hide the cursor

            // Enable player controls
        }
    }
        
}



