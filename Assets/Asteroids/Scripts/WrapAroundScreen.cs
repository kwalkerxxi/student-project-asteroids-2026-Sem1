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

    Vector3 warpTargetPosition;

    public Rigidbody cachedRigidbody;
    public Collider cachedCollider;
    public LayerMask separationMask;

    void Start()
    {
        if(cameraToDetectWrapping == null)
        {
            cameraToDetectWrapping = Camera.main;
        }

        Time.timeScale = 5f;



        if(cachedRigidbody == null)
        {
            cachedRigidbody = GetComponent<Rigidbody>();
        }

        if(cachedCollider == null)
        {
            cachedCollider = GetComponent<Collider>();
        }
    }


    public void PreSeparate(Vector3 targetPosition)
    {
        // Temporarily move the collider to the target position
        Vector3 originalPos = transform.position;
        transform.position = targetPosition;

        Collider[] hits = Physics.OverlapBox(
            cachedCollider.bounds.center,
            cachedCollider.bounds.extents,
            transform.rotation,
            separationMask
        );

        foreach(var hit in hits)
        {
            if(hit == cachedCollider)
            {
                continue;
            }

            Vector3 dir;
            float dist;

            if(Physics.ComputePenetration(
                cachedCollider, transform.position, transform.rotation,
                hit, hit.transform.position, hit.transform.rotation,
                out dir, out dist))
            {
                targetPosition += dir * dist;
            }
        }

        // Restore original position
        transform.position = originalPos;

        // Now move safely
        cachedRigidbody.MovePosition(targetPosition);
    }



    void FixedUpdate()
    {

        if(WarpSpot)
        {
            Rigidbody myRigidbody = transform.GetComponent<Rigidbody>();

            PreSeparate(warpTargetPosition);
            //myRigidbody.MovePosition(warpTargetPosition);
            WarpSpot = false;
            OnWrap?.Invoke();
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
                //transform.position = hitPoint;
                warpTargetPosition = hitPoint;
            }
            //WarpSpot = false;
        }
    }
}

