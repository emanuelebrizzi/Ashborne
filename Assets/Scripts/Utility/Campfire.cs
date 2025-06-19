using UnityEngine;

public class Campfire : MonoBehaviour, IInteractable
{
    [SerializeField] GameManager gameManager; // Reference to the GameManager
    [SerializeField] HeroSpawnManager heroSpawnManager; // Reference to the SpawnPointManager
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject campfirePrefab;
    [SerializeField] GameObject interactionIcon;
    [SerializeField] CampfireMenu campfireMenu;
    string campfireSceneName = "Campfire"; 


    bool IsInRange;

    
    void Start()
    {
        IsInRange = false; 
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false); 
        }
    }

    
    void Update()
    { 
        
        if (IsInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

    }

    public void Interact()
    {
        SetSpawnPoint();
        campfireMenu.OpenCampfireMenu(); // Open the campfire menu
        //gameManager.LoadGameScene(campfireSceneName); // Load the campfire scene
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = true; // Player is in range of the campfire
            Debug.Log("Player is in range of the campfire.");
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = false; // Player has left the range of the campfire
            Debug.Log("Player has left the range of the campfire.");
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false);
            }
        }
    }

    private void SetSpawnPoint()
    {
        // Logic to set the spawn point for the player
        if (spawnPoint != null)
        {
            heroSpawnManager.SetHeroSpawnPoint(spawnPoint);
            Debug.Log("Spawn point set");
        }
    }

    public bool IsInteractable()
    {
        return IsInRange;
    }

}
