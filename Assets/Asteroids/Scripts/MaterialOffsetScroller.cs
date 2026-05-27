using System.Collections.Generic;
using UnityEngine;

public class MaterialOffsetScroller : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] List<Material> materials;

    [Header("Scroll Speed (UV units per second)")]
    [SerializeField] float[] scrollX;
    [SerializeField] float[] scrollY;
    public bool IsScrolling = true;

    private void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
                materials.AddRange(renderer.materials);
        }
    }

    void Update()
    {
        if (!IsScrolling)
        {
            return;
        }


        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i] == null) continue;

            Vector2 delta = new Vector2(scrollX[i%scrollX.Length], scrollY[i%scrollY.Length]) * Time.deltaTime;
            materials[i].mainTextureOffset += delta;
        }
    }
}