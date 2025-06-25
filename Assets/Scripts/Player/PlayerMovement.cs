using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 500f;
    [SerializeField] Collider2D groundCheck;
    [SerializeField] float coyoteTime = 0.05f;

    float coyoteTimeCounter;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

    }

    public void Move(float direction)
    {
        body.linearVelocity = new Vector2(direction * speed, body.linearVelocity.y);
        Player.Instance.PlayAnimation(PlayerState.MOVE, 0);
        if (direction  > 0)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        else if (direction  < 0)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }

    }
    
    public void Stop()
        {
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);
            Player.Instance.PlayAnimation(PlayerState.IDLE, 0);
        }

    public void Jump()
    {
        if (coyoteTimeCounter > 0f)
        {
            body.AddForce(new Vector2(0, jumpForce));
            coyoteTimeCounter = 0f;
        }
    }

    private bool IsGrounded()
    {
        return groundCheck.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    public void IncreaseSpeed(float value)
    {
        speed += value;
    }

}