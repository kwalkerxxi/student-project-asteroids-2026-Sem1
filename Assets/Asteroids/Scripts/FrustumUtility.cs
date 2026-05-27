using UnityEngine;

public static class FrustumUtility
{
    /// <summary>
    /// Checks if a transform's position is inside the given camera's frustum.
    /// </summary>
    /// <param name="cameraToUSe">The camera to test against.</param>
    /// <param name="target">The transform to check.</param>
    /// <returns>True if inside frustum, false otherwise.</returns>
    public static bool IsTransformInFrustum(Camera cameraToUSe, Transform target)
    {
        if(cameraToUSe == null || target == null)
        {
            Debug.LogWarning("IsTransformInFrustum: Camera or Transform is null.");
            return false;
        }

        // Convert world position to viewport coordinates
        Vector3 viewportPos = cameraToUSe.WorldToViewportPoint(target.position);

        // Check if the point is in front of the camera and inside the viewport
        return viewportPos.z > 0 &&
               viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f;
    }
}
