using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
    [SerializeField] Player player;
    [SerializeField] Transform defaultRespawnPoint;
    Transform lastCheckpoint = null;

    private void Start()
    {
        if (playerObject == null)
        {
            playerObject = Player.Instance.gameObject;
            player = Player.Instance;
        }

        if (defaultRespawnPoint == null)
        {
            Debug.LogWarning("Default respawn point is not set. Player will not be repositioned on death.");
        }
    }
    public void Die()
    {
        if (playerObject == null)
        {
            Debug.LogError("Player GameObject is not assigned.");
            return;
        }

        // Play death animation
        Player.Instance.PlayAnimation(PlayerState.DEATH);

        DropAshEchoes();
        Respawn();
    
    }
    private void Respawn()
    {
        if (lastCheckpoint != null)
        {
            playerObject.transform.position = lastCheckpoint.position;
        }
        else if (defaultRespawnPoint != null)
        {
            playerObject.transform.position = defaultRespawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No respawn point set. Player will not be repositioned.");
        }
        player.Health.ResetHealth();
    }

    private void DropAshEchoes()
    {
        AshEchoes ashEchoes = player.AshEchoes;

        if (ashEchoes != null)
        {
            ashEchoes.DropEchoes(player.transform);
        }
        else
        {
            Debug.LogWarning("AshEchoes component not found on player. No echoes dropped.");
        }
    }

    public void SetLastCheckpoint(Transform checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

}

