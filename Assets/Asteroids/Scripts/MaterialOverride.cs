using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialOverride : MonoBehaviour
{
    [Header("Material Settings")]
    [SerializeField] private Material[] buildingColors;

    [Tooltip("Which material index to override")]
    [SerializeField] private int materialIndex = 0;

    [SerializeField] float zChangeDistance = 5;
    private Renderer rendererComponent;

   
    private void Awake()
    {
        rendererComponent = GetComponent<Renderer>();
    }

    private void Start()
    {
        ApplyMaterial();
    }

    public void ApplyMaterial()
    {
        if (rendererComponent == null)
        {
            Debug.LogWarning("Renderer not found.");
            return;
        }

        if (buildingColors == null)
        {
            Debug.LogWarning("No material assigned.");
            return;
        }

        Material[] materials = rendererComponent.materials;

        if (materialIndex < 0 || materialIndex >= materials.Length)
        {
            Debug.LogWarning($"Material index {materialIndex} is out of range.");
            return;
        }

        Material buildingMaterialToUse = buildingColors[0];
        for (int i = 0; i < buildingColors.Length; i++)
        {
            var min = (zChangeDistance * i); 
            var max = zChangeDistance * (i + 2);

            //Debug.Log(i + " " + min + " " + max);
            if (transform.localPosition.z >= min  && transform.localPosition.z < max)
            {
                buildingMaterialToUse = buildingColors[i];
                break;
            }
        }
       
        materials[materialIndex] = buildingMaterialToUse;
        rendererComponent.materials = materials;
    }
}