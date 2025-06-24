using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Player player;
    Rigidbody2D body;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 500f;
    Collider2D groundCheck;

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
        if (IsGrounded())
        {
            body.AddForce(new Vector2(0, jumpForce));
        }
    }

    private bool IsGrounded()
    {
        return groundCheck.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    public void IncreaseFlatSpeed(float value)
    {
        speed += value;
    }

}