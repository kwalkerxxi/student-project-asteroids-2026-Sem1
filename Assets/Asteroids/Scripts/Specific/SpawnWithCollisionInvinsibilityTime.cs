using System.Collections;
using UnityEngine;

public class SpawnWithCollisionInvinsibilityTime : MonoBehaviour
{
    [SerializeField] private float invulnerabilityTime = 1f;
    [SerializeField] private string layerNameAfterIframesEnd = "Asteroid";
    void Start()
    {
        //gameObject.layer = LayerMask.NameToLayer("SpawnedItem");
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("SpawnedItem"));
        StartCoroutine(EnableCollisions());
    }

    IEnumerator EnableCollisions()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        //gameObject.layer = LayerMask.NameToLayer(layerNameAfterIframesEnd);
        SetLayerRecursively(gameObject, LayerMask.NameToLayer(layerNameAfterIframesEnd));
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
