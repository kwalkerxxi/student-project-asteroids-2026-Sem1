using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCollisions : MonoBehaviour
{

    [SerializeField] GameObject ParticleSystemOnDeath;
    private static GameObject particleHolder;
    public UnityEvent OnDied;

    [SerializeField] GameStartAndGameOver gameStartAndGameOver;
    private void Start()
    {
        if(particleHolder == null)
        {
            particleHolder = new GameObject("Particle Holder - Players");
        }


        gameStartAndGameOver = GameObject.FindAnyObjectByType<GameStartAndGameOver>();
        gameStartAndGameOver.RegisterPlayer(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Asteroid")
            || collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("EnemyBullet"))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Asteroid")
            || collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("EnemyBullet"))
        {
            Die();
        }
    }

    public void Die()
    {
        OnDied?.Invoke();

        // GetComponent<PlayerInput>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<PlayerScore>().DisableScore();

        //PlayerInput playerInput = GetComponent<PlayerInput>();

        //foreach(var device in playerInput.devices)
        //{
        //    if(device is Keyboard || device is Mouse)
        //    {
        //        DisableKeyboardJoining.isKeyboardJoingingAllowed = false;
        //        continue; // skip keyboard & mouse
        //    }
        //    InputSystem.DisableDevice(device);
        //    // Debug.Log($"{device.displayName} disabled");
        //}

        TransformUtils.DeleteChildren(transform, false);
        if(ParticleSystemOnDeath != null)
        {
            GameObject deathParticleSystem = Instantiate(ParticleSystemOnDeath, particleHolder.transform);
            deathParticleSystem.transform.SetPositionAndRotation(transform.position, transform.rotation);
            deathParticleSystem.AddComponent<AutoDestroyAfterParticlesEnd>();
        }


        //Destroy(gameObject);
    }
}
