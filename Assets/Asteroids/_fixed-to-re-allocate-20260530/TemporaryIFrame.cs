using UnityEngine;

public class TemporaryIFrame : MonoBehaviour
{
    Collider objectCollider;
    void Start()
    {
        objectCollider = GetComponent<Collider>();
        objectCollider.enabled = false;

        Invoke(nameof(ReEnable), Random.Range(0.04f, 0.06f));
    }

    void ReEnable()
    {
        if(objectCollider != null)
        {
            objectCollider.enabled = true;
            Destroy(this);
        }
    }
}
