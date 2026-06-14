
using UnityEngine;

/// <summary>
/// This script spawns the "asteroids" off the screen - with limits and delays
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject largePrefab;

    [Header("Object Limits")]
    int aliveCount = 0;
    [SerializeField] int maxAsteroids = 26;

    Transform asteroidHolder;

    [Header("Wave Details")]
    [SerializeField] int currentWave = 1;
    [SerializeField] int baseEnemies = 3;       // Starting number of enemies
    [SerializeField] float growthFactor = 1.3f; // 30% more enemies each wave
    [SerializeField] float spawnDelay = 0.5f;   // Delay between spawns

    [SerializeField] SpawnRotationType startingRotationType = SpawnRotationType.SquaredOff;

    private void Start()
    {
        asteroidHolder = new GameObject("Asteroid Spawner").transform;
        //SpawnWave();
        Invoke(nameof(SpawnWave), 2);
    }



    public void SpawnWave()
    {
        int amountToSpawn = Mathf.RoundToInt(baseEnemies * Mathf.Pow(growthFactor, currentWave - 1));

        for(int i = 0; i < amountToSpawn; i++)
        {

            Vector3 spawnLocation = ScreenPositionUtility.GetOffScreenPositionIndexed(
                Camera.main,
                fixedY: 0f,
                spawnIndex: i,
                totalPoints: amountToSpawn,
                offscreenPercent: 0.1f,
                spacingPercent: 0.2f
            );

            spawnLocation.y = transform.position.y;

            Quaternion spawnRotation = Quaternion.identity;

            switch(startingRotationType)
            {
                case SpawnRotationType.SquaredOff:
                    spawnRotation = ScreenPositionUtility.GetRotationTowardCameraCenter(Camera.main, spawnLocation, 0f, true);
                    break;
                case SpawnRotationType.TowardsCenter:
                    spawnRotation = ScreenPositionUtility.GetRotationTowardCameraCenter(Camera.main, spawnLocation, 0f, false);
                    break;
                case SpawnRotationType.TowardsRandomPoint:
                    spawnRotation = ScreenPositionUtility.GetRotationTowardCameraCenter(Camera.main, spawnLocation, 0f, false, true);
                    break;
                default:
                    break;
            }

            Spawn(largePrefab, spawnLocation, asteroidHolder, spawnRotation, i);
        }

    }

    enum SpawnRotationType
    {
        SquaredOff,
        TowardsCenter,
        TowardsRandomPoint
    }

    public void Spawn(GameObject prefabToUse, Vector3 spawnPosition, Transform holder, Quaternion? rotation = null, int index = -999)
    {
        if(aliveCount >= maxAsteroids)
        {
            return;
        }

        rotation = rotation ?? Quaternion.identity;


        GameObject newObjectToSpawn;
        if(index == -999)
        {
            newObjectToSpawn = Instantiate(prefabToUse, spawnPosition, (Quaternion)rotation, holder);
        }
        else
        {
            newObjectToSpawn = GameObject.FindFirstObjectByType<LaneSpawner>().SpawnAtSetSideLocationIndex(index);
        }
        Asteroid asteroidScript = newObjectToSpawn.GetComponent<Asteroid>();
        asteroidScript.OnDied += HandleAsteroidDeath;

        aliveCount++;
    }

    void HandleAsteroidDeath(Asteroid asteroid)
    {
        aliveCount--;

        if(aliveCount <= 0)
        {
            currentWave++;
            SpawnWave();
        }

        asteroid.OnDied -= HandleAsteroidDeath;

        if(asteroid.NextPrefab == null)
        {
            return;
        }

        int spawnCount = Mathf.Min(2, maxAsteroids - aliveCount);
        Vector3 dir = Random.insideUnitSphere.normalized;
        dir.y = transform.position.y;

        for(int i = 0; i < spawnCount; i++)
        {
            Spawn(asteroid.NextPrefab, (Vector3)asteroid.transform.position + (i == 0 ? dir : -dir) * 0.4f, asteroidHolder);
        }
    }
}