using UnityEngine;

public class UFOLargeShooting : MonoBehaviour
{
    //public GameObject bulletPrefab;

    [SerializeField]
    private BulletPool.PoolType desiredPoolType;
    private BulletPool bulletPool;

    public float fireRate = 2f;

    public float bulletSpeed = 8f;

    private float timer;

    private void Awake()
    {
        bool foundPool = BulletPool.TryGetPool(desiredPoolType, out bulletPool);

        if (!foundPool)
        {
            Debug.LogError($"No BulletPool registered for {desiredPoolType}");
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            timer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        float randomAngle = Random.Range(0f, 360f);

        Vector3 shootingDirection = Quaternion.Euler(0, randomAngle, 0) * Vector2.right;

        //GameObject bullet =
        //    Instantiate(
        //        bulletPrefab,
        //        transform.position,
        //        Quaternion.identity
        //    );

        //if (bullet.TryGetComponent<UFOBullet>(out UFOBullet bulletComponent))
        //{
        //    bulletComponent.SetSpeedAndDirection(bulletSpeed, shootingDirection);
        //}

        if (bulletPool == null) { return; }

        bulletPool.FireBullet(
            transform.position,
            transform.rotation,
            shootingDirection,
            bulletSpeed
        );
    }
}