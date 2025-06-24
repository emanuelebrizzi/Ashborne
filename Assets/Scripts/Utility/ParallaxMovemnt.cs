using UnityEngine;
using UnityEngine.Tilemaps;

public class ParallaxMovement : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;

    [Header("Parallax Settings")]
    [SerializeField] float parallaxEffectX = 0.5f;
    [SerializeField] float parallaxEffectY = 0f;


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

        return newPosition;
    }
}