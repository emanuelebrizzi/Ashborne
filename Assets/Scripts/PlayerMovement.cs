using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed = 5f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y);
        if (Input.GetButtonDown("Jump") && Mathf.Abs(body.linearVelocity.y) < 0.01f)
        {
            body.AddForce(Vector2.up * 300f);
        }
    }
}
