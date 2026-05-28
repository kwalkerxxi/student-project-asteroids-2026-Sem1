using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UFOBullet : MonoBehaviour
{
    private Vector3 direction = Vector3.zero;
    private float speed = 0f;
    [SerializeField] private float lifeTime = 4f;

    public void SetSpeedAndDirection(float bulletSpeed, Vector3 bulletDirection)
    {
        speed = bulletSpeed;
        direction = bulletDirection;

        SetInMotion();
    }

    void SetInMotion()
    {
        Rigidbody bulletRigidbody = GetComponent<Rigidbody>();

        bulletRigidbody.linearVelocity = direction * speed;

        Destroy(gameObject, lifeTime);
    }
}
