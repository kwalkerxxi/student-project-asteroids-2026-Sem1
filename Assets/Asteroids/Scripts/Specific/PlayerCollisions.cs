using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCollisions : MonoBehaviour
{

    [SerializeField] GameObject ParticleSystemOnDeath;
    private static GameObject particleHolder;
    public UnityEvent OnDied;

    private void Start()
    {
        if(particleHolder == null)
        {
            particleHolder = new GameObject("Particle Holder - Players");
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Asteroid"))
        {
            Die();
        }
    }

    public void Die()
    {
        OnDied?.Invoke();

        PlayerInput playerInput = GetComponent<PlayerInput>();

        foreach(var device in playerInput.devices)
        {
            if(device is Keyboard || device is Mouse)
            {
                DisableKeyboardJoining.isKeyboardJoingingAllowed = false;
                continue; // skip keyboard & mouse
            }
            InputSystem.DisableDevice(device);
            // Debug.Log($"{device.displayName} disabled");
        }


        if(ParticleSystemOnDeath != null)
        {
            GameObject deathParticleSystem = Instantiate(ParticleSystemOnDeath, particleHolder.transform);
            deathParticleSystem.transform.SetPositionAndRotation(transform.position, transform.rotation);
            deathParticleSystem.AddComponent<AutoDestroyAfterParticlesEnd>();
        }


        Destroy(gameObject);
    }
}
