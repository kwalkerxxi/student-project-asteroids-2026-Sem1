using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject largeUFOPrefab;
    public GameObject smallUFOPrefab;

    [Header("Spawn Timing")]
    public float minSpawnDelay = 1f; //8f;
    public float maxSpawnDelay = 2f; //15f;

    [Header("Difficulty")]
    public int scoreForSmallOnly = 40000;
    public int currentScoreForSpawning = 20000;

    private bool ufoAlive = false;
    float waitTimeForNextSpawn = 1f;
    private void Start()
    {
        InvokeRepeating(nameof(SpawnLoop), waitTimeForNextSpawn, waitTimeForNextSpawn);
    }

    void SpawnLoop()
    {
        if (!ufoAlive)
        {
            SpawnUFO();
            waitTimeForNextSpawn = Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    void SpawnUFO()
    {
        bool spawnSmall = ShouldSpawnSmall();

        GameObject prefab = spawnSmall ? smallUFOPrefab : largeUFOPrefab;

        Camera cam = Camera.main;

        float screenHeight = cam.orthographicSize * 2f;

        float screenWidth = screenHeight * cam.aspect;

        bool spawnLeft = Random.value > 0.5f;

        float x =
            spawnLeft
            ? (-screenWidth / 2f) - 2f
            : (screenWidth / 2f) + 2f;

        float z = Random.Range(
            -screenHeight / 2f,
            screenHeight / 2f
        );

        Vector3 spawnPos = new Vector3(x, 0f, z);

        GameObject ufo = Instantiate(prefab, spawnPos, Quaternion.identity);

        ufoAlive = true;

        UFODeathNotifier notifier = ufo.AddComponent<UFODeathNotifier>();

        notifier.spawner = this;
    }

    bool ShouldSpawnSmall()
    {
        int score = currentScoreForSpawning;

        if (score >= scoreForSmallOnly)
        {
            return true;
        }

        float chance = Mathf.InverseLerp(0, scoreForSmallOnly, score);

        return Random.value < chance;
    }

    public void UFODestroyed()
    {
        ufoAlive = false;
    }
}