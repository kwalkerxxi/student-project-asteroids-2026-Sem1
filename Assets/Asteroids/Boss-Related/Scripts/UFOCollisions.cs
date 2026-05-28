using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UFOCollisions : MonoBehaviour
{

    [SerializeField] GameObject ParticleSystemOnDeath;
    private static GameObject particleHolder;
    public UnityEvent OnDied;

    private void Start()
    {
        if(particleHolder == null)
        {
            particleHolder = new GameObject("Particle Holder - UFOs");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Asteroid")
            || collision.gameObject.CompareTag("Player")
            || collision.gameObject.CompareTag("Bullet"))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Asteroid")
             || collision.gameObject.CompareTag("Player")
             || collision.gameObject.CompareTag("Bullet"))
        {
            Die();
        }
    }

    public void Die()
    {
        OnDied?.Invoke();

        if(ParticleSystemOnDeath != null)
        {
            GameObject deathParticleSystem = Instantiate(ParticleSystemOnDeath, particleHolder.transform);
            deathParticleSystem.transform.SetPositionAndRotation(transform.position, transform.rotation);
            deathParticleSystem.AddComponent<AutoDestroyAfterParticlesEnd>();
        }

        Destroy(gameObject);
    }
}
