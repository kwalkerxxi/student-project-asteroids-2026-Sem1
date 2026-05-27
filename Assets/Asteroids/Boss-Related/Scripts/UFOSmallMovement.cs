using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UFOSmallMovement : MonoBehaviour
{
    public float speed = 5f;

    public float directionChangeInterval = 0.8f;

    private Rigidbody rb;

    private Vector2 moveDir;

    private float timer;

    private float horizontalDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        horizontalDirection =
            transform.position.x < 0 ? 1f : -1f;

        PickNewDirection();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= directionChangeInterval)
        {
            timer = 0f;
            PickNewDirection();
        }

        WrapScreen();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir * speed;
    }

    void PickNewDirection()
    {
        float vertical =
            Random.Range(-1f, 1f);

        moveDir =
            new Vector2(horizontalDirection, vertical).normalized;
    }

    void WrapScreen()
    {
        Camera cam = Camera.main;

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector3 pos = transform.position;

        if (pos.x > width) pos.x = -width;
        if (pos.x < -width) pos.x = width;

        if (pos.y > height) pos.y = -height;
        if (pos.y < -height) pos.y = height;

        transform.position = pos;
    }
}