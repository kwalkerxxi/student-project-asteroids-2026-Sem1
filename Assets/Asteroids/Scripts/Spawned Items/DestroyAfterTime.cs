using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float lifeTime = 2f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

  
}
