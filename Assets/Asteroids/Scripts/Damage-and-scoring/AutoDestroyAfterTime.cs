using UnityEngine;

public class AutoDestroyAfterTime : MonoBehaviour
{
    [field: SerializeField]
    public float LifeTime { get; set; } = 3f;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

}
