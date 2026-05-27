using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [field: SerializeField] public GameObject NextPrefab { get; private set; }

    public event Action<Asteroid> OnDied;

    Rigidbody cachedRigidbody;

    [Header("Linear Velocity Settings")]
    public float minLinearSpeed = 1f;   // Minimum speed in m/s
    public float maxLinearSpeed = 5f;   // Maximum speed in m/s

    [Header("Angular Velocity Settings")]
    public float minAngularSpeed = 10f; // Minimum angular speed in degrees/sec
    public float maxAngularSpeed = 90f; // Maximum angular speed in degrees/sec

    public bool spawnWithAngularVelocity = false;
    private void Start()
    {

        cachedRigidbody = GetComponent<Rigidbody>();

        // Assign random linear velocity
        float randomSpeed = Random.Range(minLinearSpeed, maxLinearSpeed);
        Vector3 randomDirection = Random.onUnitSphere; // Random 3D direction

        if(!spawnWithAngularVelocity)
        {
            //int rotationIndex = Random.Range(0, 4);
            //float yRotation = rotationIndex * 90f;
            //randomDirection = Quaternion.Euler(0, rotationIndex * 90f, 0) * Vector3.forward;
            //transform.rotation = Quaternion.Euler(new Vector3(0, yRotation, 0));
            randomDirection = transform.forward;
        }

        cachedRigidbody.linearVelocity = randomDirection * randomSpeed;

        if(spawnWithAngularVelocity)
        {
            // Assign random angular velocity
            Vector3 randomRotationAxis = Random.onUnitSphere; // Random rotation axis
            float randomAngularSpeedRad = Random.Range(minAngularSpeed, maxAngularSpeed) * Mathf.Deg2Rad; // Convert to radians/sec
            cachedRigidbody.angularVelocity = randomRotationAxis * randomAngularSpeedRad;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            Die();
        }
    }

    public void Die()
    {
        OnDied?.Invoke(this);

        if(GetComponent<Health>() != null)
        {
            GetComponent<Health>().TakeDamage(1_000_000_000, null);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Cheats.OnClearScreenOfEnemies += Die;
    }


    private void OnDisable()
    {
        Cheats.OnClearScreenOfEnemies -= Die;
    }
}