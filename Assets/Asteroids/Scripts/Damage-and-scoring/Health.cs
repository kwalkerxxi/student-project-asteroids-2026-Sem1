using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{



    public int maximumHealth = 3;
    int currentHealth;
    PlayerScore lastAttacker;
    [SerializeField] GameObject ParticleSystemOnDeath;
    private static GameObject particleHolder;
    public UnityEvent OnDied;
    void Awake()
    {
        currentHealth = maximumHealth;
        if (particleHolder == null)
        {
            particleHolder = new GameObject("Particle Holder - Health Death");
        }
    }
    public void TakeDamage(int damage, PlayerScore attacker)
    {
        currentHealth -= damage;
        lastAttacker = attacker;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        ScoreValue score = GetComponent<ScoreValue>();
        if (score && lastAttacker)
        {
            lastAttacker.AddScore(score.Value);
        }
        if (ParticleSystemOnDeath != null)
        {
            GameObject deathParticleSystem = Instantiate(ParticleSystemOnDeath, particleHolder.transform);
            deathParticleSystem.transform.SetPositionAndRotation(transform.position, transform.rotation);
            deathParticleSystem.AddComponent<AutoDestroyAfterParticlesEnd>();
        }
        OnDied?.Invoke();
        Destroy(gameObject);
    }
}