using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform firingLocation;
    private InputAction shootingAction;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private int maximumBulletCount = 2;
    private int currentBulletCount = 0;
    private bool infiniteBullets = false;

    [SerializeField] private float fireRate = 0.2f;
    private double nextFireTime = 0;
    private bool isHolding = false;

    public UnityEvent OnFiredShot; 


    private void Awake()
    {
        if(playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if(playerInput == null)
        {
            Debug.LogError("There was no PlayerInput found or set. Please check this");
            Destroy(this);
        }

        shootingAction = playerInput.actions["Attack"];
    }

    private void Start()
    {
        if(firingLocation == null)
        {
            firingLocation = transform;
        }
    }

    private void OnEnable()
    {
        //shootingAction.performed += Fire;
        Cheats.OnToggleInfiniteBullets += ToggleInfiniteBullets;

        shootingAction.started += OnFireStarted;
        shootingAction.canceled += OnFireCanceled;
    }
    private void OnDisable()
    {
        //shootingAction.performed -= Fire;
        Cheats.OnToggleInfiniteBullets -= ToggleInfiniteBullets;

        shootingAction.started -= OnFireStarted;
        shootingAction.canceled -= OnFireCanceled;
    }

    void ToggleInfiniteBullets()
    {
        infiniteBullets = !infiniteBullets;
    }

    private void OnFireStarted(InputAction.CallbackContext ctx)
    {
        isHolding = true;

        TryFire();
    }

    private void OnFireCanceled(InputAction.CallbackContext ctx)
    {
        isHolding = false;
    }

    void Update()
    {
        if(isHolding)
        {
            TryFire();
        }
    }

    private void TryFire()
    {
        if(Time.timeAsDouble >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.timeAsDouble + fireRate;
        }
    }

    private void Fire()
    {
        if(!infiniteBullets && currentBulletCount >= maximumBulletCount)
        {
            return;
        }

        if(!firingLocation.gameObject.activeInHierarchy)
        {
            return;
        }

        GameObject newBulletSpawned = Instantiate(bulletPrefab, firingLocation.position, firingLocation.rotation);
        Bullet bulletScript = newBulletSpawned.GetComponent<Bullet>();
        bulletScript.OnDied += HandleAsteroidDeath;
        DamageDealer dealer = bulletScript.GetComponent<DamageDealer>();
        dealer.Owner = transform.root.GetComponentInChildren<PlayerScore>();

        OnFiredShot?.Invoke();

        currentBulletCount++;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Fire();
    }

    void HandleAsteroidDeath(Bullet bullet)
    {
        currentBulletCount--;

        bullet.OnDied -= HandleAsteroidDeath;
    }

}
