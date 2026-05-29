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
    //[SerializeField] private ThumbnailData[] thumbnails;

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    foreach(var thumbnail in thumbnails)
    //    {
    //        thumbnail.rawImage.texture = ThumbnailRenderer.RenderThumbnail(thumbnail.thumbnailObject);
    //    }
    //}

    public ThumbnailRig rig;
    public GameObject prefab;

    void Start()
    {
        rig.Initialize(prefab);
    }
}
