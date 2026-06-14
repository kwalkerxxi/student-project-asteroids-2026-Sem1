using UnityEngine;

public static class ScreenBoundsChecker
{
    public enum OffScreenSide
    {
        None,
        Left,
        Right,
        Top,
        Bottom
    }

    public static OffScreenSide GetOffScreenSide(Vector3 worldPosition, Camera cam)
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(worldPosition);

        bool isBehindCamera = viewportPos.z < 0;
        if(isBehindCamera)
        {
            // Flip horizontally/vertically when behind camera if needed
            return OffScreenSide.None;
        }


        if(viewportPos.y < 0f)
        {
            return OffScreenSide.Bottom;
        }

        if(viewportPos.y > 1f)
        {
            return OffScreenSide.Top;
        }

        if(viewportPos.x < 0f)
        {
            return OffScreenSide.Left;
        }

        if(viewportPos.x > 1f)
        {
            return OffScreenSide.Right;
        }



        return OffScreenSide.None; // on screen
    }
}