using UnityEngine;
using UnityEngine.UI;

public class ThumbnailRig : MonoBehaviour
{
    [Header("UI Target")]
    public RawImage targetImage;

    [Header("Settings")]
    public int resolution = 256;
    public float rotationSpeed = 45f;

    Camera cam;
    RenderTexture rt;
    GameObject instance;
    Transform pivot;

    public void Initialize(GameObject prefab)
    {
        // Create camera
        cam = new GameObject("ThumbnailCamera").AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Color;
        cam.backgroundColor = Color.clear;

        // Create RenderTexture
        rt = new RenderTexture(resolution, resolution, 16);
        cam.targetTexture = rt;
        targetImage.texture = rt;

        // Create pivot + instance
        pivot = new GameObject("Pivot").transform;
        instance = Instantiate(prefab, pivot);

        // Frame the object
        var bounds = CalculateBounds(instance);
        float dist = bounds.extents.magnitude * 2f;
        cam.transform.position = bounds.center + Vector3.back * dist;
        cam.transform.LookAt(bounds.center);

        // Only render this object
        int layer = 31;
        SetLayerRecursively(instance, layer);
        cam.cullingMask = 1 << layer;
    }

    //void Update()
    //{
    //    if(pivot != null)
    //    {
    //        pivot.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    //    }
    //}

    Bounds CalculateBounds(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        var b = new Bounds(go.transform.position, Vector3.zero);
        foreach(var r in renderers)
        {
            b.Encapsulate(r.bounds);
        }

        return b;
    }

    void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach(Transform t in go.transform)
        {
            SetLayerRecursively(t.gameObject, layer);
        }
    }
}
