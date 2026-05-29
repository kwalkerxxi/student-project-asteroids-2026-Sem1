using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TargetCameraToRawImage
    : MonoBehaviour
{
    [Header("UI Target")]
    RawImage targetImage;

    [Header("RenderTexture Settings")]
    [SerializeField] int resolution = 256;

    Camera dynamicTemporaryCamera;
    GameObject cameraTargetObject;
    [SerializeField] private Vector3 offsetFromCamera = (Vector3.back * 5.5f) + (Vector3.up * 2f);

    [Header("Make Instance Settings")]
    [SerializeField] bool usePrefabInstance = false;
    Transform pivotPointTransform;
    [SerializeField] float rotationSpeed = 45f;
    public void Initialize(GameObject objectToRender, RawImage rawImageToRenderTo)
    {
        targetImage = rawImageToRenderTo;

        // Create camera
        dynamicTemporaryCamera = new GameObject("ThumbnailCamera").AddComponent<Camera>();
        dynamicTemporaryCamera.clearFlags = CameraClearFlags.Nothing;
        //dynamicTemporaryCamera.backgroundColor = Color.clear;
        //dynamicTemporaryCamera.clearFlags = CameraClearFlags.SolidColor;
        //dynamicTemporaryCamera.backgroundColor = new Color(0, 0, 0, 0);

        //dynamicTemporaryCamera.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.DepthNormals;
        //dynamicTemporaryCamera.renderingPath = RenderingPath.Forward;



        // Create RenderTexture
        //dynamicRenderTexture = new RenderTexture(resolution, resolution, 16);

        RenderTexture dynamicRenderTexture = new RenderTexture(resolution, resolution, 32)
        {
            graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm,
            depthStencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.D32_SFloat_S8_UInt,
            antiAliasing = 1,
            useMipMap = false,
            autoGenerateMips = false,
            enableRandomWrite = false,
            dimension = UnityEngine.Rendering.TextureDimension.Tex2D
        };

        dynamicRenderTexture.Create();

        dynamicTemporaryCamera.targetTexture = dynamicRenderTexture;
        targetImage.texture = dynamicRenderTexture;

        if(usePrefabInstance)
        {
            // Create pivot + instance
            pivotPointTransform = new GameObject("Pivot").transform;
            cameraTargetObject = Instantiate(objectToRender, pivotPointTransform);
        }
        else
        {
            cameraTargetObject = objectToRender;
        }
        // Frame the object
        var bounds = CalculateBounds(cameraTargetObject);
        dynamicTemporaryCamera.transform.position = (bounds.center) + offsetFromCamera;
        dynamicTemporaryCamera.transform.LookAt(bounds.center);

        // Only render this object
        //int layer = 31;
        //SetLayerRecursively(instance, layer);
        //cam.cullingMask = 1 << layer;
    }

    void Update()
    {
        if(!usePrefabInstance)
        {
            return;
        }

        if(pivotPointTransform != null)
        {
            pivotPointTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    Bounds CalculateBounds(GameObject gameObjectToCheck)
    {
        var renderers = gameObjectToCheck.GetComponentsInChildren<Renderer>();
        var boundsOfObject = new Bounds(gameObjectToCheck.transform.position, Vector3.zero);
        foreach(var currentRenderer in renderers)
        {
            boundsOfObject.Encapsulate(currentRenderer.bounds);
        }

        return boundsOfObject;
    }

    void SetLayerRecursively(GameObject gameObjectToChange, int layerToUse)
    {
        gameObjectToChange.layer = layerToUse;
        foreach(Transform currentTransform in gameObjectToChange.transform)
        {
            SetLayerRecursively(currentTransform.gameObject, layerToUse);
        }
    }
}
