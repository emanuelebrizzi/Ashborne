using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float horizontalSpeed = 1f;
    public float verticalSpeed = 0f;

    [Header("Looping")]
    public bool enableLooping = true;
    public float textureWidth = 10f; // Adjust based on your sprite size

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        // Auto-calculate texture width if sprite renderer exists
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            textureWidth = sr.bounds.size.x;
        }
    }

    void Update()
    {
        // Move the background
        float moveX = horizontalSpeed * Time.time;
        float moveY = verticalSpeed * Time.time;

        transform.position = startPosition + new Vector3(moveX, moveY, 0);

        // Loop horizontally
        if (enableLooping && horizontalSpeed != 0)
        {
            if (transform.position.x >= startPosition.x + textureWidth)
            {
                startPosition.x += textureWidth;
            }
            else if (transform.position.x <= startPosition.x - textureWidth)
            {
                startPosition.x -= textureWidth;
            }
        }
    }
}