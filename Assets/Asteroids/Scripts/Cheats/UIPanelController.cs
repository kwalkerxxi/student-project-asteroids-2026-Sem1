using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Vector2 onScreenPosition;
    [SerializeField] private Vector2 offScreenPosition;
    [SerializeField] private float speed = 5f;

    private bool isVisible = false;
    private Vector2 targetPosition;
    private float snapDistance = 1f; // 1 pixel threshold
    public bool isActive = true;

    [SerializeField] float initalOnScreenDelay = 0f;
    void Start()
    {
        isActive = false;
        Invoke(nameof(SetOffScreen), initalOnScreenDelay);
    }

    void SetOffScreen()
    {
        isActive = true;
        //panel.anchoredPosition = offScreenPosition;
        targetPosition = offScreenPosition;
    }

    void Update()
    {
        if(!isActive)
        {
            return;
        }

        // Smoothly move panel
        panel.anchoredPosition = Vector2.Lerp(
            panel.anchoredPosition,
            targetPosition,
            Time.deltaTime * speed
        );

        // Snap when close enough
        if(Vector2.Distance(panel.anchoredPosition, targetPosition) <= snapDistance)
        {
            isActive = false;
            panel.anchoredPosition = targetPosition;
        }
    }

    public void TogglePanel()
    {
        isVisible = !isVisible;
        isActive = true;
        targetPosition = isVisible ? onScreenPosition : offScreenPosition;
    }

    public void TogglePanelOn()
    {
        isVisible = false;
        TogglePanel();
    }
}