using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private float speed = 5f;
    private float jumpForce = 300f;
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

        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y);
        if (Input.GetButtonDown("Jump") && Mathf.Abs(body.linearVelocity.y) < 0.01f)
        {
            body.AddForce(Vector2.up * jumpForce);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            direction();
            spumPrefabs.PlayAnimation(PlayerState.MOVE, 0);
        }
        else
        {
            spumPrefabs.PlayAnimation(PlayerState.IDLE, 0);
        }
    }
    
    private void direction()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
    }

}
