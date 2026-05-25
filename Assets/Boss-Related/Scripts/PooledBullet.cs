using UnityEngine;

public class PooledBullet : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 5f;

    private BulletPool owningPool;

    public void SetPool(BulletPool pool)
    {
        owningPool = pool;
    }

    private void OnEnable()
    {
        CancelInvoke();

        Invoke(nameof(ReturnToPool), lifeTime);
    }

    public void ReturnToPool()
    {
        if (owningPool != null)
        {
            owningPool.ReturnBullet(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReturnToPool();
    }
}