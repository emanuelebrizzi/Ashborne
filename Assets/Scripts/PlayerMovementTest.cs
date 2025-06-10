using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public delegate void HealthChangedHandler(float currentHealth, float maxHealth);
    public event HealthChangedHandler OnHealthChanged;

    private Rigidbody2D rb;
    private Vector2 input;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input.normalized * moveSpeed;
    }
}
