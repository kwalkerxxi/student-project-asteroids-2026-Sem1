using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class VehicleSensor : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float frontDotThreshold = 0.5f;

    [field: SerializeField]
    private readonly HashSet<Transform> nearbyVehicles = new();

    [field: SerializeField]
    public Transform ClosestVehicleAhead { get; private set; }

    public float ClosestDistanceSqr { get; private set; }

    [field: SerializeField]
    public bool HasVehicleAhead => ClosestVehicleAhead != null;

    [SerializeField]
    float zDiff = 10;

    [SerializeField]
    float pauseTime = -1;


    private void Update()
    {
        if(pauseTime <= 0)
        {
            return;
        }

        if(Time.time > pauseTime + 20)
        {
            GetComponentInParent<Asteroid>().Unpause();
            GetComponentInParent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
            //ClosestVehicleAhead = null;
            Destroy(gameObject);
        }
    }
    private void Check()
    {
        ClosestVehicleAhead = null;
        ClosestDistanceSqr = float.MaxValue;

        Vector3 myPosition = transform.position;
        Vector3 myForward = transform.forward;


        foreach(Transform vehicle in nearbyVehicles)
        {
            if(vehicle == null)
            {
                continue;
            }

            Vector3 offset = vehicle.position - myPosition;

            float dot = Vector3.Dot(
                myForward,
                offset.normalized);

            if(dot < frontDotThreshold)
            {
                continue;
            }

            float distanceSqr = offset.sqrMagnitude;

            if(distanceSqr < ClosestDistanceSqr)
            {
                ClosestDistanceSqr = distanceSqr;
                ClosestVehicleAhead = vehicle;
            }
        }


        if(ClosestVehicleAhead) //HasVehicleAhead)
        {
            zDiff = (transform.forward.z + ClosestVehicleAhead.forward.z);
            if(zDiff <= 1)
            {
                GetComponentInParent<Asteroid>().Pause();
                pauseTime = Time.time;
            }
        }


        if(ClosestVehicleAhead == null)
        {
            zDiff = 10;
            GetComponentInParent<Asteroid>().Unpause();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Asteroid"))
        {
            return;
        }

        if(other.transform == transform)
        {
            return;
        }

        nearbyVehicles.Add(other.transform);

        Check();
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Asteroid"))
        {
            return;
        }

        nearbyVehicles.Remove(other.transform);

        Check();
    }

    private void OnDrawGizmosSelected()
    {
        if(ClosestVehicleAhead == null)
        {
            return;
        }

        Gizmos.DrawLine(
            transform.position,
            ClosestVehicleAhead.position);
    }
}