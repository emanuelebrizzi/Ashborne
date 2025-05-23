using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    const float CameraDefaultZValue = -10f;
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new(0f, 1f, 0f);
    [SerializeField] float smoothSpeed = 5f;

    [SerializeField] Tilemap tilemap;
    float halfHeight;
    float halfWidth;
    Vector3 minBounds;
    Vector3 maxBounds;

    void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        Bounds bounds = tilemap.localBounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;
    }


    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        transform.position = new(clampedX, clampedY, CameraDefaultZValue);
    }
}
