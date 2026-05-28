using UnityEngine;

public class UFOSmallShooting : MonoBehaviour
{
    // public GameObject bulletPrefab;

    [SerializeField]
    private BulletPool.PoolType desiredPoolType;
    private BulletPool bulletPool;


    [SerializeField] float fireRate = 1f;

    [SerializeField] float bulletSpeed = 10f;

    [Header("Aim Error")]
    [SerializeField] float maxAimError = 25f;

    [SerializeField] float minAimError = 3f;

    [SerializeField] int perfectAccuracyScore = 40000;
    [SerializeField] int currentAccuracyScore = 10000;

    private float timer;

    private Transform playerToTarget;

    private void Awake()
    {
        bool foundPool = BulletPool.TryGetPool(desiredPoolType, out bulletPool);

        if(!foundPool)
        {
            Debug.LogError($"No BulletPool registered for {desiredPoolType}");
        }
    }


    private void Start()
    {
        //GameObject playerFoundInScene = GameObject.FindGameObjectWithTag("Player");
        Transform playerFoundInScene = PlayerJoiningBehaviour.RandomPlayerToTarget;

        if(playerFoundInScene != null)
        {
            playerToTarget = playerFoundInScene.transform;
        }
    }

    private void Update()
    {
        if(playerToTarget == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= fireRate)
        {
            Debug.Log("Time Bullet Released " + Time.time + "Fire Rate" + fireRate);
            timer = 0f;
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        Vector3 shootingDirection = (playerToTarget.position - transform.position).normalized;

        float accuracy = Mathf.InverseLerp(0, perfectAccuracyScore, currentAccuracyScore);

        float currentError = Mathf.Lerp(maxAimError, minAimError, accuracy);

        float randomOffset = Random.Range(-currentError, currentError);

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

        if(bulletPool == null)
        { return; }

        bulletPool.FireBullet(
            transform.position,
            transform.rotation,
            shootingDirection,
            bulletSpeed
        );

    }
}