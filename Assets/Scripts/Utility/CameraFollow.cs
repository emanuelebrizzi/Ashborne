using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Vector3 offset = new(0f, 2f, -10f);
    [SerializeField] float smoothTime = 0.25f;


    Vector3 currentVelocity = Vector3.zero;
    float halfHeight;
    float halfWidth;
    Vector3 minBounds;
    Vector3 maxBounds;

    void Start()
    {
        InitalizeCameraDimensions();
        InitializeBounds();
        ClampPositionToBounds(target.position + offset);
    }

    void InitalizeCameraDimensions()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }
    void InitializeBounds()
    {
        Bounds bounds = tilemap.localBounds;
        minBounds = tilemap.transform.TransformPoint(bounds.min);
        maxBounds = tilemap.transform.TransformPoint(bounds.max);
    }

    void ClampPositionToBounds(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        transform.position = new Vector3(clampedX, clampedY, position.z);
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

        ClampPositionToBounds(smoothedPosition);
    }
}