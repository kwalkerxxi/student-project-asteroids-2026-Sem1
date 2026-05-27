using UnityEngine;

public static class ScreenCheckUtility
{
    public static bool IsTransformOnScreen(Camera cameraToUse, Transform target)
    {
        if(cameraToUse == null || target == null)
        {
            return false;
        }

        Vector3 screenPos = cameraToUse.WorldToScreenPoint(target.position);

        return screenPos.z > 0 && // In front of camera
               screenPos.x >= 0 && screenPos.x <= Screen.width &&
               screenPos.y >= 0 && screenPos.y <= Screen.height;
    }
}
