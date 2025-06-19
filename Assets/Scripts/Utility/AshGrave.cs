using UnityEngine;

public class AshGrave : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject ashPrefab; // Reference to the ash prefab
    [SerializeField] private GameObject interactionIcon; // Reference to the interaction icon
    [SerializeField] private AshEchoes ashEchoes; // Reference to the AshEchoes component
    [SerializeField] private int echoCount = 0; // Number of ash echoes 
    bool IsInRange;
    void Start()
    {
        IsInRange = false;
        ashPrefab.SetActive(false);
        interactionIcon.SetActive(false);
    }
    void Update()
    {
        if (IsInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = true; // Player is in range of the ash
            Debug.Log("Player is in range of the ash.");
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
            IsInRange = false; // Player has left the range of the ash
            Debug.Log("Player has left the range of the ash.");
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false);
            }
        }
    }

    public void Interact()
    {
        ashPrefab.SetActive(false);
        ashEchoes.AddEchoes(echoCount);
        echoCount = 0;
    }

    public void DropAshEchoes(Transform dropPoint, int amount)
    {
        ashPrefab.transform.position = new Vector3(dropPoint.position.x, dropPoint.position.y, 0);
        ashPrefab.SetActive(true);
        echoCount = amount; 
    }

    public bool IsInteractable()
    {
        return IsInRange;
    }

}
