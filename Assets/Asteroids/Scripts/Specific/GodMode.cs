using System.Collections.Generic;
using UnityEngine;

public class GodMode : MonoBehaviour
{
    [SerializeField] Material[] materialsOfPlayer;
    [SerializeField] Material godModeMaterial;


    Material[][] originalMaterials;   // store per-renderer materials
    Renderer[] renderers;
        List<Renderer> rendererToChange = new List<Renderer>();

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        
        for (int i = 0; i < renderers.Length; i++)
        {
            if (!renderers[i].TryGetComponent<ParticleSystem>(out ParticleSystem ps))
            {
                rendererToChange.Add(renderers[i]);
            }
        }

        originalMaterials = new Material[renderers.Length][];

        if(rendererToChange == null || rendererToChange.Count <= 0)
        {
            return;
        }

        for(int i = 0; i < rendererToChange.Count; i++)
        {
            originalMaterials[i] = rendererToChange[i].materials;
        }
    }

    void ToggleMataerials(bool isInGodMode)
    {
        if(isInGodMode)
        {
            ApplyGodModeMaterials();
            return;
        }

        RestoreMaterials();
    }

    void ApplyGodModeMaterials()
    {

        if(rendererToChange == null || rendererToChange.Count <= 0)
        {
            return;
        }

        foreach(var renderer in rendererToChange)
        {
            Material[] materials = renderer.materials;
            for(int i = 0; i < materials.Length; i++)
            {
                materials[i] = godModeMaterial;
            }
            renderer.materials = materials;
        }
    }

    void RestoreMaterials()
    {
        if(rendererToChange == null || rendererToChange.Count <= 0)
        {
            return;
        }

        for(int i = 0; i < rendererToChange.Count; i++)
        {
            rendererToChange[i].materials = originalMaterials[i];
        }
    }

    void ToggleCollider(bool isInGodMode)
    {
        Collider collider = GetComponent<Collider>();

        collider.enabled = !collider.enabled;
    }

    private void OnEnable()
    {
        Cheats.OnToggleGodMode += ToggleCollider;
        Cheats.OnToggleGodMode += ToggleMataerials;
    }

    private void OnDisable()
    {
        Cheats.OnToggleGodMode -= ToggleCollider;
        Cheats.OnToggleGodMode -= ToggleMataerials;

    }
}
