using UnityEngine;

/// <summary>
/// This script allows for spawning various items (randomly) at various set locations
/// </summary>
public class GreebleSpawner : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    [SerializeField] private GameObject[] itemsToUse;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private float scaleMultiplier = 1f;

   public void Initiate(float changedValue)
    {
        scaleMultiplier = changedValue;
        SpawnItems();
    }

    
    void SpawnItems()
    {
        if(itemsToUse.Length == 0 || spawnLocations.Length == 0)
        {
            Debug.LogWarning("Spawner: Missing items or spawn locations.");
            return;
        }

        foreach(Transform spawnPoint in spawnLocations)
        {
            int randomIndex = Random.Range(0, itemsToUse.Length);
            GameObject itemToSpawn = itemsToUse[randomIndex];


            int rotationIndex = Random.Range(0, 4);
            float yRotation = rotationIndex * 90f;
            Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);
            Quaternion finalRotation = spawnPoint.rotation * rotation;

            GameObject itemSpawned = Instantiate(itemToSpawn, spawnPoint.position, finalRotation, transform);
            itemSpawned.transform.localScale = Vector3.one * scaleMultiplier;
        }
    }
}
