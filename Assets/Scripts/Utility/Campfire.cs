using UnityEngine;

public class Campfire : MonoBehaviour, IInteractable
{

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
            Interact();
        }

    }

    public void Interact()
    {

        //SetSpawnPoint();
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
            //.Instance.SetSpawnPoint(spawnPoint.position);
            Debug.Log("Spawn point set to: " + spawnPoint.position);
        }
    }

    public bool IsInteractable()
    {
        return IsInRange;
    }

}
