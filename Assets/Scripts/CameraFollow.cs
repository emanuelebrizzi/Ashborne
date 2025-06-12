using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new(0f, 2f, -10f);
    [SerializeField] float smoothTime = 0.25f;
    float halfHeight;
    float halfWidth;
    Vector3 minBounds;
    Vector3 maxBounds;
    Vector3 currentVelocity;

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
        Vector3 targetPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);

        float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);
    }
}
