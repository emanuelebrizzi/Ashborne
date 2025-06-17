using UnityEngine;

public class CameraParallax : MonoBehaviour
{
    [Header("Camera Reference")]
    public Transform cameraTransform; // Drag Main Camera here

    [Header("Parallax Settings")]
    public float parallaxEffectX = 0.5f;
    public float parallaxEffectY = 0f;

    [Header("Repeating Background")]
    public bool enableRepeating = true;
    public float textureWidth = 10f; // Width of your background sprite
    public float textureHeight = 10f; // Height of your background sprite (for vertical repeating)

    [Header("Optional: Bounds Limiting")]
    public bool limitBounds = false;
    public Vector2 minBounds = Vector2.zero;
    public Vector2 maxBounds = Vector2.zero;

    private Vector3 lastCameraPosition;
    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Auto-find camera if not assigned
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (cameraTransform != null)
            lastCameraPosition = cameraTransform.position;

        startPosition = transform.position;

        // Get sprite renderer and auto-calculate texture size
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            textureWidth = spriteRenderer.bounds.size.x;
            textureHeight = spriteRenderer.bounds.size.y;
        }
    }

    void LateUpdate() // Use LateUpdate to run after camera movement
    {
        if (cameraTransform == null) return;

        // Calculate camera movement since last frame
        Vector3 cameraMovement = cameraTransform.position - lastCameraPosition;

        // Apply parallax effect (less than 1.0 makes it move slower than camera)
        Vector3 parallaxMovement = new Vector3(
            cameraMovement.x * parallaxEffectX,
            cameraMovement.y * parallaxEffectY,
            0
        );

        // Move the background layer
        Vector3 newPosition = transform.position + parallaxMovement;

        // Handle repeating background
        if (enableRepeating)
        {
            // Horizontal repeating
            if (textureWidth > 0)
            {
                float cameraX = cameraTransform.position.x;
                float backgroundX = newPosition.x;

                // Check if background has moved too far from camera
                if (Mathf.Abs(backgroundX - cameraX) > textureWidth)
                {
                    // Snap background to repeat seamlessly
                    float offset = Mathf.Round((backgroundX - cameraX) / textureWidth) * textureWidth;
                    newPosition.x = cameraX + (backgroundX - cameraX - offset);
                }
            }

            // Vertical repeating (less common, but available)
            if (textureHeight > 0 && parallaxEffectY != 0)
            {
                float cameraY = cameraTransform.position.y;
                float backgroundY = newPosition.y;

                if (Mathf.Abs(backgroundY - cameraY) > textureHeight)
                {
                    float offset = Mathf.Round((backgroundY - cameraY) / textureHeight) * textureHeight;
                    newPosition.y = cameraY + (backgroundY - cameraY - offset);
                }
            }
        }

        // Optional: Clamp to bounds if enabled (usually not used with repeating)
        if (limitBounds && !enableRepeating)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = newPosition;

        // Update last camera position for next frame
        lastCameraPosition = cameraTransform.position;
    }

    // Reset to starting position (useful for testing)
    [ContextMenu("Reset Position")]
    void ResetPosition()
    {
        transform.position = startPosition;
        if (cameraTransform != null)
            lastCameraPosition = cameraTransform.position;
    }

    // Auto-calculate texture size from sprite
    [ContextMenu("Auto Calculate Texture Size")]
    void AutoCalculateTextureSize()
    {
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            textureWidth = spriteRenderer.bounds.size.x;
            textureHeight = spriteRenderer.bounds.size.y;
            Debug.Log($"Auto-calculated size: {textureWidth} x {textureHeight}");
        }
    }
}