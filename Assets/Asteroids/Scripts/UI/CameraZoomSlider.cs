using UnityEngine;
using UnityEngine.UI;

public class CameraZoomSlider : MonoBehaviour
{
    [Header("Camera to move")]
    public Camera targetCamera;

    [Header("UI Slider")]
    public Slider zoomSlider;

    [Header("Movement Settings")]
    public float minDistance = -10f;
    public float maxDistance = 10f;

    private Vector3 startPosition;

    private void Start()
    {
        if (targetCamera == null)
        {
            Debug.LogError("No camera assigned.");
            return;
        }

        // Store original position
        startPosition = targetCamera.transform.position;

        // Configure slider
        zoomSlider.minValue = 0f;
        zoomSlider.maxValue = 1f;

        // Listen for slider changes
        zoomSlider.onValueChanged.AddListener(UpdateCameraPosition);

        // Apply initial position
        UpdateCameraPosition(zoomSlider.value);
    }

    private void UpdateCameraPosition(float sliderValue)
    {
        // Convert slider value into movement distance
        float distance = Mathf.Lerp(minDistance, maxDistance, sliderValue);

        // Move along camera's forward direction
        targetCamera.transform.position =
            startPosition + targetCamera.transform.forward * distance;
    }
}
