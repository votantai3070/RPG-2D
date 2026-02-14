using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPosX;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        CalculateImageLength();
    }

    private void FixedUpdate()
    {
        float currentCameraPosX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPosX - lastCameraPosX;
        lastCameraPosX = currentCameraPosX;

        float cameraLeftEdge = mainCamera.transform.position.x - cameraHalfWidth;
        float cameraRightEdge = mainCamera.transform.position.x + cameraHalfWidth;

        foreach (var layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void CalculateImageLength()
    {
        foreach (var layer in backgroundLayers)
        {
            layer.CalculateImageWidth();
        }
    }
}
