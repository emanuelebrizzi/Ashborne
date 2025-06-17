using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;

    [Header("Parallax Settings")]
    [SerializeField] float parallaxEffectX = 0.5f;
    [SerializeField] float parallaxEffectY = 0f;

    // TODO: After the tilemap of the main level is completed, we can assign these bounds
    [Header("Optional: Bounds Limiting")]
    public bool limitBounds = false;
    public Vector2 minBounds = Vector2.zero;
    public Vector2 maxBounds = Vector2.zero;

    Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (cameraTransform != null)
            lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = ComputeNewPosition();
        transform.position = newPosition;
        lastCameraPosition = cameraTransform.position;
    }

    Vector3 ComputeNewPosition()
    {
        Vector3 cameraMovement = cameraTransform.position - lastCameraPosition;
        Vector3 parallaxMovement = new(
            cameraMovement.x * parallaxEffectX,
            cameraMovement.y * parallaxEffectY,
            0
        );

        Vector3 newPosition = transform.position + parallaxMovement;

        if (limitBounds)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        }

        return newPosition;
    }
}