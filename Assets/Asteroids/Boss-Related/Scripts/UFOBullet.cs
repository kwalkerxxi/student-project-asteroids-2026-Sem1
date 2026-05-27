using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UFOBullet : MonoBehaviour
{
    private Vector3 direction = Vector3.zero;
    private float speed = 0f;

    public void SetSpeedAndDirection(float bulletSpeed, Vector3 bulletDirection)
    {
        speed = bulletSpeed;
        direction = bulletDirection;

        SetInMotion();
    }

    void SetInMotion()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.linearVelocity = direction * speed;

        Destroy(gameObject, 4f);
    }
}
