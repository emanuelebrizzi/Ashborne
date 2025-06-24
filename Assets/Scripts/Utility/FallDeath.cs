using UnityEngine;

public class FallDeath : MonoBehaviour
{
    [SerializeField] PlayerDeathHandler playerDeathHandler;
    [SerializeField] Transform gravelSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDeathHandler.FallDeath(gravelSpawnPoint);
        }
    }

}