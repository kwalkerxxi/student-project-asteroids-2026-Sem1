using UnityEngine;

public class AdjustAnchorsToCorners : MonoBehaviour
{
    public void AdjustAnchors()
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            // Get the object's local position and size
            Vector2 localPosition = rt.localPosition;
            Vector2 size = rt.rect.size;

            // Get the parent's RectTransform to calculate the relative position
            RectTransform parentRect = rt.parent.GetComponent<RectTransform>();

            // Get the parent's width and height
            float parentWidth = parentRect.rect.width;
            float parentHeight = parentRect.rect.height;

            // Calculate the normalized values for anchorMin and anchorMax based on the object's local position and size
            float left = localPosition.x - size.x * 0.5f;
            float right = localPosition.x + size.x * 0.5f;
            float bottom = localPosition.y - size.y * 0.5f;
            float top = localPosition.y + size.y * 0.5f;

            // Normalize the coordinates with respect to the parent's size
            float normalizedLeft = left / parentWidth;
            float normalizedRight = right / parentWidth;
            float normalizedBottom = bottom / parentHeight;
            float normalizedTop = top / parentHeight;

            // Set the anchors based on the normalized corner values
            rt.anchorMin = new Vector2(normalizedLeft, normalizedBottom);
            rt.anchorMax = new Vector2(normalizedRight, normalizedTop);

            // Optionally, reset anchoredPosition if necessary
            rt.anchoredPosition = Vector2.zero; // Keep the object's position relative to its new anchors
        }
    }
}
