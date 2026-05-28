using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Exposes UnityEvents in the Inspector and invokes them when global actions are triggered.
/// </summary>
public class AspectRatioEvents : MonoBehaviour
{
    public UnityEvent OnStandard;
    public UnityEvent OnUltraWide;
    public UnityEvent OnVeryWide;

    void OnEnable()
    {
        AspectRatioLogic.OnStandard += InvokeOnStandard;
        AspectRatioLogic.OnUltraWide += InvokeOnUltraWide;
        AspectRatioLogic.OnVeryWide += InvokeOnVeryWide;
    }

    void OnDisable()
    {
        AspectRatioLogic.OnStandard -= InvokeOnStandard;
        AspectRatioLogic.OnUltraWide -= InvokeOnUltraWide;
        AspectRatioLogic.OnVeryWide -= InvokeOnVeryWide;
    }

    void InvokeOnStandard()
    {
        OnStandard?.Invoke();
    }

    void InvokeOnUltraWide()
    {
        OnUltraWide?.Invoke();
    }

    void InvokeOnVeryWide()
    {
        OnVeryWide?.Invoke();
    }

    [Header("UI Adjustment")]
    [SerializeField] private RectTransform targetUI;
    [SerializeField] private Camera targetCameraToRotate;

    public void AdjustRectTransformPositionY(float positionY)
    {
        Vector2 position = targetUI.anchoredPosition;
        position.y = positionY;
        targetUI.anchoredPosition = position;
    }

    public void AdjustRectTransformPositionX(float positionX)
    {
        Vector2 position = targetUI.anchoredPosition;
        position.x = positionX;
        targetUI.anchoredPosition = position;
    }

    public void AdjustCameraRotationX(float rotationX)
    {
        Vector3 rotation = targetCameraToRotate.transform.rotation.eulerAngles;
        rotation.x = rotationX;
        targetCameraToRotate.transform.rotation = Quaternion.Euler(rotation);
    }
}