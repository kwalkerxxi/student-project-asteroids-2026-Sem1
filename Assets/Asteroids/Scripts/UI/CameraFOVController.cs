using UnityEngine;
using UnityEngine.UI;

public class CameraFOVController : MonoBehaviour
{
    [Header("Assign all cameras here")]
    public Camera[] cameras;

    [Header("UI Slider")]
    public Slider fovSlider;

    [Header("FOV Range")]
    public float minFOV = 30f;
    public float maxFOV = 120f;

    private void Start()
    {
        // Configure slider range
        fovSlider.minValue = minFOV;
        fovSlider.maxValue = maxFOV;

        // Set slider to first camera's FOV if available
        if (cameras.Length > 0 && cameras[0] != null)
        {
            fovSlider.value = cameras[0].fieldOfView;
        }

        // Add listener
        fovSlider.onValueChanged.AddListener(UpdateAllCameraFOVs);

        // Apply initial value
        UpdateAllCameraFOVs(fovSlider.value);
    }

    private void UpdateAllCameraFOVs(float newFOV)
    {
        foreach (Camera camera in cameras)
        {
            if (camera != null)
            {
                camera.fieldOfView = newFOV;
            }
        }
    }
}
