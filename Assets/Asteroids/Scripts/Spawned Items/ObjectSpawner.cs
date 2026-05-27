using UnityEngine;
using UnityEngine.AdaptivePerformance;


/// <summary>
/// This contains several spawner methods - to be used with UnityEvents
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
     
    public static void SpawnObjectOnScreen(GameObject whatToSpawn, Transform holderTransform = null)
    {
        Vector3 spawnLocation = ScreenPositionUtility.GetRandomOnScreenPosition(Camera.main, 0, 0.8f);
        GameObject newObjectToSpawn = Instantiate(whatToSpawn, spawnLocation, Quaternion.identity, holderTransform);
    }

    public static void SpawnObjectOffScreen(GameObject whatToSpawn, Transform holderTransform = null)
    {
        Vector3 spawnLocation = ScreenPositionUtility.GetRandomOffScreenPosition(Camera.main, 0, 0.1f);
        GameObject newObjectToSpawn = Instantiate(whatToSpawn, spawnLocation, Quaternion.identity, holderTransform);
    }
}
