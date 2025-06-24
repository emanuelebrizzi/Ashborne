using UnityEngine;

public class Campfire : MonoBehaviour, IInteractable
{
    [SerializeField] HeroSpawnManager heroSpawnManager; // Reference to the SpawnPointManager
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject campfirePrefab;
    [SerializeField] GameObject interactionIcon;

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
            SetSpawnPoint();
            GameManager.Instance.OpenCampfire();
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
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

    void OnTriggerExit2D(Collider2D collision)
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

    void SetSpawnPoint()
    {
        // Logic to set the spawn point for the player
        if (spawnPoint != null)
        {
            heroSpawnManager.SetHeroSpawnPoint(spawnPoint);
            Debug.Log("Spawn point set");
        }
    }

    public void Interact()
    {

    }

    public bool IsInteractable()
    {
        return IsInRange;
    }

}
