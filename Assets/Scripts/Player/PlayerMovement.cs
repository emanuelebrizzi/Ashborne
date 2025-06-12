using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed = 5f;
    private float jumpForce = 500f;
    private Collider2D groundCheck;

    public Player player;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<Collider2D>();

        if (player == null)
        {
            player = GetComponent<Player>();
        }
    }


    void Update()
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
            player.PlayAnimation(PlayerState.IDLE, 0);
        }

    }

    private void Direction()
    {
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y);
        player.PlayAnimation(PlayerState.MOVE, 0);
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
        return groundCheck.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

}