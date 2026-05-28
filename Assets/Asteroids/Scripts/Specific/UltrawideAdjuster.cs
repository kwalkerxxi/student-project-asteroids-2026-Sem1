using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class UltrawideAdjuster : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera targetCamera;

    [Header("FOV Settings")]
    [SerializeField] private float normalFOV = 60f;     // 16:9 or smaller
    [SerializeField] private float ultrawideFOV = 75f;  // Wider than 16:10

    [Header("UI Adjustment")]
    [SerializeField] private RectTransform targetUI;
    [SerializeField] private float normalPosX = -50f;
    [SerializeField] private float ultrawidePosX = -200f;

    float lastAspect = -1f;
    float aspect;
    private void Start()
    {
        ApplyAdjustments();
    }

    private void Update()
    {
        aspect = (float)Screen.width / Screen.height;

        // Only update if aspect ratio changed
        if(!Mathf.Approximately(aspect, lastAspect))
        {
            lastAspect = aspect;
            ApplyAdjustments();
        }
    }


    public void ApplyAdjustments()
    {
        //aspect = (float)Screen.width / Screen.height;
        // lastAspect = aspect;

        // 16:10 aspect ratio = 1.6
        bool isUltrawide = aspect > 1.6f;

        // Camera FOV
        if(targetCamera != null)
        {
            targetCamera.fieldOfView = isUltrawide
                ? ultrawideFOV
                : normalFOV;
        }

        // UI X Position
        if(targetUI != null)
        {
            Vector2 pos = targetUI.anchoredPosition;

            pos.x = isUltrawide
                ? ultrawidePosX
                : normalPosX;

            targetUI.anchoredPosition = pos;
        }
    }
}