using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BuildingGridSpawn : MonoBehaviour
{
    [Header("Prefabs & Settings")]
    [SerializeField] GameObject[] buildingPrefabsToUse;
    [SerializeField] float spacingX = 5;
    [SerializeField] float spacingZ = 5;
    [SerializeField] int itemsOnX = 10;
    [SerializeField] int itemsOnZ = 15;
    [SerializeField] float[] rotationValues = { 0f, 90f, 180f, 270f };
    


    [Header("GPU Instancing")]
    [SerializeField] bool useGPUInstancing = false;

    // Internal instancing data
    private Dictionary<Mesh, List<Matrix4x4>> instanceBatches = new Dictionary<Mesh, List<Matrix4x4>>();
    private Dictionary<Mesh, Material> meshMaterials = new Dictionary<Mesh, Material>();

    private void Start()
    {
        if(useGPUInstancing)
        {
            PrepareGPUInstances();
        }
        else
        {
            SpawnNormally();
        }
    }

    private void SpawnNormally()
    {
        for(int i = 0; i < itemsOnX; i++)
        {
            for(int j = 0; j < itemsOnZ; j++)
            {
                GameObject prefabPicked = buildingPrefabsToUse[Random.Range(0, buildingPrefabsToUse.Length)];

                Vector3 spotToSpawnAt = transform.position;
                spotToSpawnAt.x += i * spacingX;
                spotToSpawnAt.z += j * spacingZ;

                float rotationPicked = rotationValues[Random.Range(0, rotationValues.Length)];

                Quaternion rotation = Quaternion.Euler(0f, rotationPicked, 0f);

                GameObject building = Instantiate(prefabPicked, spotToSpawnAt, rotation, transform);

                float buildingScale = Random.Range(0.8f, 1.2f);
                building.transform.localScale = Vector3.one * buildingScale;

                GreebleSpawner[] greebleSpawnersInside = building.GetComponentsInChildren<GreebleSpawner>();
                if (greebleSpawnersInside != null)
                {
                    foreach (var greebleSpawner in greebleSpawnersInside)
                    {
                        greebleSpawner.Initiate(buildingScale);
                    }
                }

                building.layer = gameObject.layer;
            }
        }
    }

    private void PrepareGPUInstances()
    {
        foreach(var prefab in buildingPrefabsToUse)
        {
            Mesh mesh = prefab.GetComponentInChildren<MeshFilter>().sharedMesh;
            Material mat = prefab.GetComponentInChildren<MeshRenderer>().sharedMaterial;

            if(!mat.enableInstancing)
            {
                Debug.LogWarning($"{mat.name} does NOT have GPU Instancing enabled!");
            }

            if(!instanceBatches.ContainsKey(mesh))
            {
                instanceBatches[mesh] = new List<Matrix4x4>();
                meshMaterials[mesh] = mat;
            }
        }

        for(int i = 0; i < itemsOnX; i++)
        {
            for(int j = 0; j < itemsOnZ; j++)
            {
                GameObject prefabPicked = buildingPrefabsToUse[Random.Range(0, buildingPrefabsToUse.Length)];

                Mesh mesh = prefabPicked.GetComponentInChildren<MeshFilter>().sharedMesh;

                Vector3 pos = transform.position + new Vector3(i * spacingX, 0, j * spacingZ);
                Quaternion rot = Quaternion.Euler(-90f, 0f, 0f);
                Vector3 scale = Vector3.one * 20f;

                Matrix4x4 matrix = Matrix4x4.TRS(pos, rot, scale);
                instanceBatches[mesh].Add(matrix);
            }
        }
    }

    private void Update()
    {
        if(!useGPUInstancing)
        {
            return;
        }

        foreach(var kvp in instanceBatches)
        {
            Mesh mesh = kvp.Key;
            Material mat = meshMaterials[mesh];
            List<Matrix4x4> matrices = kvp.Value;

            // Draw in batches of 1023 (Unity limit)
            for(int i = 0; i < matrices.Count; i += 1023)
            {
                int batchSize = Mathf.Min(1023, matrices.Count - i);
                Graphics.DrawMeshInstanced(mesh, 0, mat, matrices.GetRange(i, batchSize));
            }
        }
    }
}
