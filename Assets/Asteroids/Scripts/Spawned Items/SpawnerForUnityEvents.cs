using Unity.VisualScripting;
using UnityEngine;

public class SpawnerForUnityEvents : MonoBehaviour
{
    public void SpawnItem(GameObject pickupToSpawn)
    {
        ObjectSpawner.SpawnObjectOnScreen(pickupToSpawn, transform);
    }

}
