using UnityEngine;

public class UFOSmallShooting : MonoBehaviour
{
    // public GameObject bulletPrefab;

    [SerializeField]
    private BulletPool.PoolType desiredPoolType;
    private BulletPool bulletPool;


    public float fireRate = 1f;

    public float bulletSpeed = 10f;

    [Header("Aim Error")]
    public float maxAimError = 25f;

    public float minAimError = 3f;

    public int perfectAccuracyScore = 40000;
    public int currentAccuracyScore = 10000;

    private float timer;

    private Transform player;

    private void Awake()
    {
        bool foundPool = BulletPool.TryGetPool(desiredPoolType, out bulletPool);

        if (!foundPool)
        {
            Debug.LogError($"No BulletPool registered for {desiredPoolType}");
        }
    }


    private void Start()
    {
        GameObject p =
            GameObject.FindGameObjectWithTag("Player");

        if (p != null)
            player = p.transform;
    }

    private void Update()
    {
        if (player == null)
            return;

        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            timer = 0f;
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        Vector3 shootingDirection =
            (player.position - transform.position).normalized;

        float accuracy =
            Mathf.InverseLerp(
                0,
                perfectAccuracyScore,
                currentAccuracyScore
            );

        float currentError =
            Mathf.Lerp(
                maxAimError,
                minAimError,
                accuracy
            );

        float randomOffset =
            Random.Range(
                -currentError,
                currentError
            );

        shootingDirection = Quaternion.Euler(0, randomOffset, 0) * shootingDirection;

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