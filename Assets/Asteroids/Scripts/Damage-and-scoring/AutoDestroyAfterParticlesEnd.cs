using UnityEngine;

/// <summary>
/// IsAlive(true) includes child particle systems automatically
/// Works for looping or non-looping systems
/// No need to track durations manually
/// </summary>
public class AutoDestroyAfterParticlesEnd : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(particleSystem != null && !particleSystem.IsAlive(true))
        {
            Destroy(gameObject);
        }
    }
}
