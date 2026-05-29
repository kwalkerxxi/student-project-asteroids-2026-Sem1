using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ThumbnailData
{
    public RawImage rawImage;
    public GameObject thumbnailObject;
}

public class CharacterThumbnails : MonoBehaviour
{
    [SerializeField] private ThumbnailData[] thumbnails;

    [SerializeField] private TargetCameraToRawImage targetCameraToThumbnails;

    void Start()
    {
        foreach(var thumbnail in thumbnails)
        {
            targetCameraToThumbnails.Initialize(thumbnail.thumbnailObject, thumbnail.rawImage);
        }
    }
}
