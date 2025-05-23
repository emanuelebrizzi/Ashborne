using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, 0f);
    [SerializeField] private float smoothSpeed = 5f;

    [SerializeField] private Tilemap tilemap;
    private float halfHeight;
    private float halfWidth;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        Bounds bounds = tilemap.localBounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;
    }

    // LateUpdte is called after all Update() calls are finished
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, -10f);
    }
}
