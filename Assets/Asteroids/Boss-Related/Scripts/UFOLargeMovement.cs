using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UFOLargeMovement : MonoBehaviour
{
    public float speed = 3f;

    public float directionChangeInterval = 1.5f;

    private Rigidbody cachedRigidbody;

    private Vector3 moveDirection;

    private float timer;

    private float horizontalDirection;

    private void Start()
    {
        cachedRigidbody = GetComponent<Rigidbody>();

        horizontalDirection = transform.position.x < 0 ? 1f : -1f;

        PickNewDirection();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= directionChangeInterval)
        {
            timer = 0f;
            PickNewDirection();
        }
    }

    private void FixedUpdate()
    {
        cachedRigidbody.linearVelocity = moveDirection * speed;
    }

    void PickNewDirection()
    {
        float vertical = Random.Range(-1f, 1f);

        moveDirection = new Vector3(horizontalDirection, 0, vertical).normalized;
    }

}
