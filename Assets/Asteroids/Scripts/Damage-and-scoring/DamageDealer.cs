using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 1;
    public PlayerScore Owner { get; set; }

    void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();

        if(health)
        {
            health.TakeDamage(damage, Owner);
        }

        Destroy(gameObject);
    }
}