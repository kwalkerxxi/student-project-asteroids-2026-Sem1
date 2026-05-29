using UnityEngine;

public class ThumbnailRenderer
{
    public static Texture2D RenderThumbnail(GameObject prefab, int size = 256)
    {
        // 1. Create temporary camera
        var camGO = new GameObject("ThumbnailCamera");
        var cam = camGO.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Color;
        cam.backgroundColor = Color.clear;

        // 2. Create RenderTexture
        var rt = new RenderTexture(size, size, 16);
        cam.targetTexture = rt;

        // 3. Spawn the object in a temporary layer
        var obj = GameObject.Instantiate(prefab);
        obj.layer = 31; // your custom "Thumbnail" layer
        cam.cullingMask = 1 << 31;

        // 4. Auto‑frame the object
        var bounds = CalculateBounds(obj);
        cam.transform.position = bounds.center + Vector3.back * bounds.extents.magnitude * 2f;
        cam.transform.LookAt(bounds.center);

        // 5. Render
        cam.Render();

        // 6. Convert to Texture2D
        RenderTexture.active = rt;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, size, size), 0, 0);
        tex.Apply();

        // 7. Cleanup
        RenderTexture.active = null;
        GameObject.DestroyImmediate(obj);
        GameObject.DestroyImmediate(camGO);
        rt.Release();

        return tex;
    }

    static Bounds CalculateBounds(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        var b = new Bounds(go.transform.position, Vector3.zero);
        foreach(var r in renderers)
        {
            b.Encapsulate(r.bounds);
        }

        return b;
    }
}
