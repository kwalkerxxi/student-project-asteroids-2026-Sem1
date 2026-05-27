using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script wraps objects to the oppsite side of the screen
/// </summary>
public class WrapAroundScreen : MonoBehaviour
{
    [SerializeField] private Camera cameraToDetectWrapping;

    private Vector3 newPos = new Vector2(-99, -99);
    private Vector3 currentScreenPosition;
    private bool WarpSpot = false;

    public UnityEvent OnWrap = new UnityEvent();

    void Start()
    {
        if(cameraToDetectWrapping == null)
        {
            cameraToDetectWrapping = Camera.main;
        }
    }

    void Update()
    {
        currentScreenPosition = cameraToDetectWrapping.WorldToViewportPoint(transform.position);
        newPos = currentScreenPosition;

        if(currentScreenPosition.y > 1.05f)
        {
            newPos.y = -0.025f;
            WarpSpot = true;
        }
        else if(currentScreenPosition.y < -0.05f)
        {
            newPos.y = 1.025f;
            WarpSpot = true;
        }
        if(currentScreenPosition.x > 1.05f)
        {
            newPos.x = -0.025f;
            WarpSpot = true;
        }
        else if(currentScreenPosition.x < -0.05f)
        {
            newPos.x = 1.025f;
            WarpSpot = true;
        }

        if(WarpSpot)
        {
            //Does not work when camera rotated!
            //transform.position = cameraToDetectWrapping.ViewportToWorldPoint(newPos);

            // Ray from camera through the viewport point
            Ray ray = cameraToDetectWrapping.ViewportPointToRay(newPos);

            // Plane at fixed world Y (for example, Y=0)
            Plane plane = new Plane(Vector3.up, Vector3.up * 0f); // change 0f to whatever Y you want

            if(plane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                transform.position = hitPoint;
            }
            WarpSpot = false;

            OnWrap?.Invoke();
        }
    }
}

