using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody cachedRigidbody;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float bulletLife = 1f;
    public event Action<Bullet> OnDied;
    void Start()
    {
        cachedRigidbody = GetComponent<Rigidbody>();
        cachedRigidbody.linearVelocity = transform.forward * speed;
        Destroy(gameObject, bulletLife);
    }

    //private void Update()
    //{
    //    if(!FrustumUtility.IsTransformInFrustum(Camera.main, transform))
    //    {
    //        Die();
    //    }
    //}

    private void OnDestroy()
    {
        Die();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Die();
    }

    public void Die()
    {
        OnDied?.Invoke(this);
        Destroy(gameObject);
    }
}
