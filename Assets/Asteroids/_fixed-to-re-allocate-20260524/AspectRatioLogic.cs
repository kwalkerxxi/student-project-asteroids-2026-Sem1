using System;
using UnityEngine;

/// <summary>
/// Detects the aspect ratio of the screen and invokes global actions based on the ratio.
/// </summary>
public class AspectRatioLogic : MonoBehaviour
{
    public float ultraWideMin = 32f / 9f;
    public float ultraWideMax = 48f / 9f;
    public float veryWideMin = 48f / 9f;

    [SerializeField]
    private int lastWidth = 0;
    [SerializeField]
    private int lastHeight = 0;

    /// <summary>
    /// Global actions invoked when the aspect ratio changes.
    /// </summary>
    public static Action OnStandard;
    public static Action OnUltraWide;
    public static Action OnVeryWide;

    void Start()
    {
        CheckAspectRatio();
        lastWidth = Camera.main.pixelWidth;
    }

    void Update()
    {
        int currentWidth = Camera.main.pixelWidth;
        int currentHeight = Camera.main.pixelHeight;
        if(currentWidth != lastWidth || currentHeight != lastHeight)
        {
            CheckAspectRatio();
            lastWidth = currentWidth;
            lastHeight = currentHeight;
        }
    }

    void CheckAspectRatio()
    {
        int width = Camera.main.pixelWidth;
        int height = Camera.main.pixelHeight;
        float aspect = (float)width / height;

        if(aspect >= ultraWideMin && aspect <= ultraWideMax)
        {
            OnUltraWide?.Invoke();
            Debug.Log("UltraWide");
        }
        else if(aspect >= veryWideMin)
        {
            OnVeryWide?.Invoke();
            Debug.Log("OnVeryWide");
        }
        else
        {
            OnStandard?.Invoke();
            Debug.Log("OnStandard");
        }
    }
}