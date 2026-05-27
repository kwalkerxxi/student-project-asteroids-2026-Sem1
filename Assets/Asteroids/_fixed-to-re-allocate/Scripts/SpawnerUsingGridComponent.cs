using UnityEngine;

/// <summary>
/// This spawns objects using the Grid component for spacing and location
/// </summary>
public class SpawnerUsingGridComponent : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private Vector2Int gridSize;

    void Start()
    {
        SpawnBuildings();
    }

    void SpawnBuildings()
    {
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector3 worldPos = grid.CellToWorld(cellPos);

                GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

                int rotationIndex = Random.Range(0, 4);
                float yRotation = rotationIndex * 90f;
                Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);


                Instantiate(prefab, worldPos, rotation, transform);
            }
        }
    }
}
