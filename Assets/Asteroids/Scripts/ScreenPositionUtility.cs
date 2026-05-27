using UnityEngine;

/// <summary>
/// This class contains helper methods to postion objects on the screen
/// </summary>
public static class ScreenPositionUtility
{
    private enum ScreenSide
    {
        Top,
        Bottom,
        Left,
        Right
    }

    /// <summary>
    /// Returns a random world position within the camera's visible area at a fixed Y height.
    /// Works for both orthographic and perspective cameras. Ideal for Top Down view.
    /// </summary>
    /// <param name="cameraToUse">The camera to use (usually Camera.main).</param>
    /// <param name="fixedY">The Y height to place the object at.</param>
    public static Vector3 GetRandomOnScreenPosition(Camera cameraToUse, float fixedY)
    {
        if(cameraToUse == null)
        {
            Debug.LogError("Camera is null in GetRandomOnScreenPosition.");
            return Vector3.zero;
        }

        // Create a horizontal plane at the given Y height
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));

        // Bottom-left world position
        Ray rayBL = cameraToUse.ViewportPointToRay(new Vector3(0, 0, 0));
        groundPlane.Raycast(rayBL, out float enterBL);
        Vector3 worldBL = rayBL.GetPoint(enterBL);

        // Top-right world position
        Ray rayTR = cameraToUse.ViewportPointToRay(new Vector3(1, 1, 0));
        groundPlane.Raycast(rayTR, out float enterTR);
        Vector3 worldTR = rayTR.GetPoint(enterTR);

        // Random X and Z within bounds
        float randomX = Random.Range(worldBL.x, worldTR.x);
        float randomZ = Random.Range(worldBL.z, worldTR.z);

        return new Vector3(randomX, fixedY, randomZ);
    }

    public static Vector3 GetRandomOnScreenPosition(Camera cameraToUse, float fixedY, float safePercent = 0.8f)
    {
        if(cameraToUse == null)
        {
            Debug.LogError("Camera is null in GetRandomOnScreenPosition.");
            return Vector3.zero;
        }

        // Calculate safe borders
        float border = (1f - safePercent) * 0.5f; // e.g., (1 - 0.8) / 2 = 0.1

        float minV = border;
        float maxV = 1f - border;

        // Pick a random viewport point inside the safe zone
        Vector3 viewportPoint = new Vector3(
            Random.Range(minV, maxV),
            Random.Range(minV, maxV),
            0f
        );

        // Raycast from camera to plane at fixedY
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = cameraToUse.ViewportPointToRay(viewportPoint);

        if(groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }

    public static Vector3 GetRandomOffScreenPosition(Camera cameraToUse, float fixedY, float offscreenPercent = 0.1f)
    {
        if(cameraToUse == null)
        {
            Debug.LogError("Camera is null in GetRandomOffScreenPosition.");
            return Vector3.zero;
        }

        // offscreenPercent = how far outside the screen to spawn (0.1 = 10% outside)
        float min = 0f - offscreenPercent;
        float max = 1f + offscreenPercent;

        // Choose a side: 0 = left, 1 = right, 2 = bottom, 3 = top
        int side = Random.Range(0, 4);

        Vector3 viewportPoint = Vector3.zero;

        switch(side)
        {
            case 0: // Left
                viewportPoint = new Vector3(min, Random.Range(0f, 1f), 0f);
                break;

            case 1: // Right
                viewportPoint = new Vector3(max, Random.Range(0f, 1f), 0f);
                break;

            case 2: // Bottom
                viewportPoint = new Vector3(Random.Range(0f, 1f), min, 0f);
                break;

            case 3: // Top
                viewportPoint = new Vector3(Random.Range(0f, 1f), max, 0f);
                break;
        }

        // Raycast from camera to plane at fixedY
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = cameraToUse.ViewportPointToRay(viewportPoint);

        if(groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }


    public static Vector3 GetOffScreenPositionIndexed(
    Camera cameraToUse,
    float fixedY,
    int spawnIndex,
    int totalPoints,
    float offscreenPercent = 0.1f,
    float spacingPercent = 0f)
    {
        if(cameraToUse == null)
        {
            Debug.LogError("Camera is null in GetOffScreenPositionIndexed.");
            return Vector3.zero;
        }

        // Clamp to avoid errors
        spawnIndex = Mathf.Clamp(spawnIndex, 0, totalPoints - 1);

        float min = 0f - offscreenPercent;
        float max = 1f + offscreenPercent;

        // Choose a side: 0 = left, 1 = right, 2 = bottom, 3 = top
        int side = Random.Range(0, 4);

        // Compute evenly spaced t value (0–1)
        float spacing = 1f / totalPoints;
        float t = (spawnIndex * spacing) + (spacing * spacingPercent);

        Vector3 viewportPoint = Vector3.zero;

        switch(side)
        {
            case 0: // Left
                viewportPoint = new Vector3(min, t, 0f);
                break;

            case 1: // Right
                viewportPoint = new Vector3(max, t, 0f);
                break;

            case 2: // Bottom
                viewportPoint = new Vector3(t, min, 0f);
                break;

            case 3: // Top
                viewportPoint = new Vector3(t, max, 0f);
                break;
        }

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = cameraToUse.ViewportPointToRay(viewportPoint);

        if(groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }



    public static Vector3 GetOffScreenPositionAroundEdgeIndexed(
    Camera cameraToUse,
    float fixedY,
    int spawnIndex,
    int totalPoints,
    float offscreenPercent = 0.1f)
    {
        if(cameraToUse == null)
        {
            Debug.LogError("Camera is null in GetOffScreenPositionAroundEdgeIndexed.");
            return Vector3.zero;
        }

        spawnIndex = Mathf.Clamp(spawnIndex, 0, totalPoints - 1);

        // Normalized position around perimeter (0–1)
        float t = (float)spawnIndex / totalPoints;

        float min = 0f - offscreenPercent;
        float max = 1f + offscreenPercent;

        Vector3 viewportPoint = Vector3.zero;

        // Perimeter is divided into 4 equal segments:
        // 0.00–0.25 = bottom
        // 0.25–0.50 = right
        // 0.50–0.75 = top
        // 0.75–1.00 = left

        if(t < 0.25f)
        {
            // Bottom edge
            float lerp = t / 0.25f;
            viewportPoint = new Vector3(Mathf.Lerp(0f, 1f, lerp), min, 0f);
        }
        else if(t < 0.5f)
        {
            // Right edge
            float lerp = (t - 0.25f) / 0.25f;
            viewportPoint = new Vector3(max, Mathf.Lerp(0f, 1f, lerp), 0f);
        }
        else if(t < 0.75f)
        {
            // Top edge
            float lerp = (t - 0.5f) / 0.25f;
            viewportPoint = new Vector3(Mathf.Lerp(0f, 1f, lerp), max, 0f);
        }
        else
        {
            // Left edge
            float lerp = (t - 0.75f) / 0.25f;
            viewportPoint = new Vector3(min, Mathf.Lerp(0f, 1f, lerp), 0f);
        }

        // Project onto ground plane
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = cameraToUse.ViewportPointToRay(viewportPoint);

        if(groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }


    public static Vector3 GetOffScreenPositionFromLargerPool(
    Camera cameraToUse,
    float fixedY,
    int spawnIndex,
    int totalPoints,
    int virtualTotalPoints,
    float offscreenPercent = 0.1f,
    float jitterPercent = 0f)
    {
        if(cameraToUse == null)
        {
            Debug.LogError("Camera is null in GetOffScreenPositionFromLargerPool.");
            return Vector3.zero;
        }

        spawnIndex = Mathf.Clamp(spawnIndex, 0, totalPoints - 1);

        // Map real index into virtual perimeter
        float mappedIndex = (float)spawnIndex / totalPoints * virtualTotalPoints;

        // Add jitter (optional)
        mappedIndex += Random.Range(-jitterPercent, jitterPercent) * virtualTotalPoints;

        // Wrap around
        mappedIndex = Mathf.Repeat(mappedIndex, virtualTotalPoints);

        // Convert to normalized perimeter position
        float t = mappedIndex / virtualTotalPoints;

        float min = 0f - offscreenPercent;
        float max = 1f + offscreenPercent;

        Vector3 viewportPoint = Vector3.zero;

        if(t < 0.25f)
        {
            float lerp = t / 0.25f;
            viewportPoint = new Vector3(Mathf.Lerp(0f, 1f, lerp), min, 0f);
        }
        else if(t < 0.5f)
        {
            float lerp = (t - 0.25f) / 0.25f;
            viewportPoint = new Vector3(max, Mathf.Lerp(0f, 1f, lerp), 0f);
        }
        else if(t < 0.75f)
        {
            float lerp = (t - 0.5f) / 0.25f;
            viewportPoint = new Vector3(Mathf.Lerp(0f, 1f, lerp), max, 0f);
        }
        else
        {
            float lerp = (t - 0.75f) / 0.25f;
            viewportPoint = new Vector3(min, Mathf.Lerp(0f, 1f, lerp), 0f);
        }

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = cameraToUse.ViewportPointToRay(viewportPoint);

        if(groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }




    //public static Quaternion GetRotationTowardCameraCenter(
    //Camera cameraToUse,
    //Vector3 fromTransformPosition,
    //float fixedY)
    //{
    //    if(cameraToUse == null || fromTransformPosition == null)
    //    {
    //        Debug.LogError("Null reference in GetRotationTowardCameraCenter.");
    //        return Quaternion.identity;
    //    }

    //    // Ray from center of camera viewport
    //    Ray ray = cameraToUse.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

    //    // Ground plane at fixedY
    //    Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, fixedY, 0f));

    //    if(!groundPlane.Raycast(ray, out float center))
    //    {
    //        return Quaternion.identity;
    //    }

    //    // Point on the ground plane directly under camera center
    //    Vector3 targetPoint = ray.GetPoint(center);

    //    // Direction from transform to target, ignoring vertical tilt
    //    Vector3 direction = targetPoint - fromTransformPosition;
    //    direction.y = 0f;

    //    if(direction.sqrMagnitude < 0.0001f)
    //    {
    //        return Quaternion.identity;
    //    }

    //    return Quaternion.LookRotation(direction);
    //}




    public static Quaternion GetRotationTowardCameraCenter(
    Camera cameraToUse,
    Vector3 fromPosition,
    float fixedY,
    bool squareOff = false, bool useRandomCenter = false)
    {
        if(cameraToUse == null)
        {
            Debug.LogError("Camera is null in GetRotationTowardCameraCenter.");
            return Quaternion.identity;
        }

        // Ray from center of camera viewport
        Ray ray = cameraToUse.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if(useRandomCenter)
        {
            ray = cameraToUse.ViewportPointToRay(new Vector3(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), 0));
        }

        // Ground plane at fixedY
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, fixedY, 0f));

        if(!groundPlane.Raycast(ray, out float enter))
        {
            return Quaternion.identity;
        }

        // Point on the ground plane directly under camera center
        Vector3 centerPoint = ray.GetPoint(enter);

        if(!squareOff)
        {
            // Normal mode: look directly at center
            Vector3 dir = centerPoint - fromPosition;
            dir.y = 0f;

            if(dir.sqrMagnitude < 0.0001f)
            {
                return Quaternion.identity;
            }

            return Quaternion.LookRotation(dir);
        }

        // --- SQUARE-OFF MODE ---
        // Determine which side of the viewport the object is on
        Vector3 viewportPos = cameraToUse.WorldToViewportPoint(fromPosition);

        // Compare distances to edges
        float leftDist = viewportPos.x;
        float rightDist = 1f - viewportPos.x;
        float bottomDist = viewportPos.y;
        float topDist = 1f - viewportPos.y;

        // Find smallest → closest edge
        float min = Mathf.Min(leftDist, rightDist, bottomDist, topDist);

        Vector3 faceDir = Vector3.forward; // default

        if(min == leftDist)
        {
            faceDir = Vector3.right;        // object is left → face right
        }
        else if(min == rightDist)
        {
            faceDir = Vector3.left;         // object is right → face left
        }
        else if(min == bottomDist)
        {
            faceDir = Vector3.forward;      // object is bottom → face up screen
        }
        else if(min == topDist)
        {
            faceDir = Vector3.back;         // object is top → face down screen
        }

        return Quaternion.LookRotation(faceDir);
    }


    //public static Vector3 GetBasicRandomOffScreenPosition()
    //{
    //    Camera cam = Camera.main;

    //    // Random X just outside screen (left or right)
    //    float x = Random.value < 0.5f ? -0.5f : 1.5f; // normalized viewport coordinates
    //    float y = Random.value; //Random.Range(0f, 1f); // within vertical viewport

    //    // Depth from camera
    //    float z = 10f; // distance from camera

    //    // Convert viewport to world position
    //    Vector3 viewportPos = new Vector3(x, y, z);
    //    Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
    //    return worldPos;
    //}

    //public static Vector3 GetRandomOffScreenPosition(Camera cameraToUse)
    //{
    //    ScreenSide side = (ScreenSide)Random.Range(0, 4);
    //    Vector3 screenSpawnPosition = GetSpawnPosition(side, cameraToUse, 1, 20);
    //    Vector3 worldSpawnPosition = Camera.main.ViewportToWorldPoint(screenSpawnPosition);
    //    return worldSpawnPosition;
    //}
    //private static Vector3 GetSpawnPosition(ScreenSide side, Camera cameraToUse, float spawnDistanceOutsideView, float spawnDistanceFromCamera)
    //{

    //    switch(side)
    //    {
    //        case ScreenSide.Top:
    //            return new Vector3(Random.value, 1f + spawnDistanceOutsideView, cameraToUse.nearClipPlane + spawnDistanceFromCamera);
    //        case ScreenSide.Bottom:
    //            return new Vector3(Random.value, -spawnDistanceOutsideView, cameraToUse.nearClipPlane + spawnDistanceFromCamera);
    //        case ScreenSide.Left:
    //            return new Vector3(-spawnDistanceOutsideView, Random.value, cameraToUse.nearClipPlane + spawnDistanceFromCamera);
    //        case ScreenSide.Right:
    //            return new Vector3(1f + spawnDistanceOutsideView, Random.value, cameraToUse.nearClipPlane + spawnDistanceFromCamera);
    //        default:
    //            return Vector3.zero;
    //    }
    //}

    public static Vector3 GetBehindCameraPosition(float distanceBehindCamera = 10f)
    {
        Camera cam = Camera.main;

        // Place player behind the camera along the forward vector
        float distanceBehind = distanceBehindCamera; // tweak for how “far off-screen” it should be
        Vector3 behindPos = cam.transform.position - cam.transform.forward * distanceBehind;

        // Optional: keep the same Y as the player
        //behindPos.y = fixedY;

        return behindPos;
    }
}
