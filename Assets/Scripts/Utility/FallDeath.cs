using UnityEngine;

public class FallDeath : MonoBehaviour
{
    [SerializeField] Transform gravelSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.Die(gravelSpawnPoint);
        }
    }

}