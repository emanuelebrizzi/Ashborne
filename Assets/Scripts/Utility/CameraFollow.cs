using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new(0f, 2f, -10f);
    [SerializeField] float smoothTime = 0.25f;
    [SerializeField] Tilemap tilemap;

    Vector3 currentVelocity = Vector3.zero;
    private float halfHeight;
    private float halfWidth;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        InitializeBounds();

        Vector3 initialPosition = target.position + offset;
        transform.position = ClampPositionToBounds(initialPosition);
    }

    void InitializeBounds()
    {
        Bounds bounds = tilemap.localBounds;
        minBounds = tilemap.transform.TransformPoint(bounds.min);
        maxBounds = tilemap.transform.TransformPoint(bounds.max);
    }

    Vector3 ClampPositionToBounds(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        return new Vector3(clampedX, clampedY, position.z);
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: No target assigned to follow.");
            return;
        }

        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );

        Vector3 boundedPosition = ClampPositionToBounds(smoothedPosition);
        transform.position = boundedPosition;
    }
}