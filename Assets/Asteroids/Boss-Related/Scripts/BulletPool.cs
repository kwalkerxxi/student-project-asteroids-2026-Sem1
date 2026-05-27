using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public enum PoolType
    {
        Player,
        Enemy,
        Boss,
        NPC,
        Turret
    }

    // Avoiding use of Singleton but using Static registry of pools
    private static readonly Dictionary<PoolType, BulletPool>
        RegisteredPools = new();

    [Header("Pool Identity")]
    [SerializeField]
    private PoolType poolType;

    [Header("Pool Settings")]
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private int defaultCapacity = 20;

    [SerializeField]
    private int maxSize = 100;

    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        // Register this pool
        RegisteredPools[poolType] = this;

        pool = new ObjectPool<GameObject>(
            CreateBullet,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            defaultCapacity,
            maxSize
        );

        PrewarmPool();
    }
    private void PrewarmPool()
    {
        List<GameObject> bullets = new();

        // Create bullets
        for (int i = 0; i < defaultCapacity; i++)
        {
            GameObject bullet = pool.Get();

            bullets.Add(bullet);
        }

        // Return bullets
        foreach (GameObject bullet in bullets)
        {
            pool.Release(bullet);
        }
    }


    private void OnDestroy()
    {
        // Remove from registry if destroyed
        if (
            RegisteredPools.TryGetValue(
                poolType,
                out BulletPool registeredPool
            )
            && registeredPool == this
        )
        {
            RegisteredPools.Remove(poolType);
        }
    }

    public static bool TryGetPool(
        PoolType poolType,
        out BulletPool bulletPool
    )
    {
        return RegisteredPools.TryGetValue(
            poolType,
            out bulletPool
        );
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.SetActive(false);

        if (!bullet.TryGetComponent<PooledBullet>(out _))
        {
            bullet.AddComponent<PooledBullet>();
        }

        return bullet;
    }

    private void OnTakeFromPool(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    private void OnReturnedToPool(GameObject bullet)
    {
        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;

        if (bullet.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            rigidbody.Sleep();
        }

        bullet.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject bullet)
    {
        Destroy(bullet);
    }

    public GameObject FireBullet(
        Vector3 position,
        Quaternion rotation,
        Vector3 direction,
        float bulletSpeed
    )
    {
        GameObject bullet = pool.Get();

        bullet.transform.position = position;
        bullet.transform.rotation = rotation;

        if (
            bullet.TryGetComponent<PooledBullet>(
                out PooledBullet pooledBullet
            )
        )
        {
            pooledBullet.SetPool(this);
        }

        if (bullet.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            rigidbody.linearVelocity =
                direction.normalized * bulletSpeed;
        }

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        pool.Release(bullet);
    }
}