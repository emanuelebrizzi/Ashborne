using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed = 5f;
    private float jumpForce = 500f;
    private SPUM_Prefabs spumPrefabs;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spumPrefabs = FindObjectOfType<SPUM_Prefabs>();
    }

    private void Start()
    {
        spumPrefabs.OverrideControllerInit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            Direction();
        }
        else
        {
            spumPrefabs.PlayAnimation(PlayerState.IDLE, 0);
        }

    }

    private void Direction()
    {
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y);
        spumPrefabs.PlayAnimation(PlayerState.MOVE, 0);
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }

    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.AddForce(new Vector2(0, jumpForce));
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }

}
