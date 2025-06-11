using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0;
    private SPUM_Prefabs spumPrefabs;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {

        playerMovement = GetComponent<PlayerMovement>();
        spumPrefabs = FindObjectOfType<SPUM_Prefabs>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && cooldownTimer > attackCooldown)
        {
            Attack();
        }
        cooldownTimer += Time.deltaTime;
    }
    private void Attack()
    {
        cooldownTimer = 0f;
        spumPrefabs.PlayAnimation(PlayerState.ATTACK, 1);

    }

}   
