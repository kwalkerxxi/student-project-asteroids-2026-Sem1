using UnityEngine;

namespace Hoey.Examples
{
    /// <summary>
    /// Spawns items outside the screen edges and moves them toward the center, with a gradually increasing spawn rate.
    /// </summary>
    public class OffScreenSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float spawnDistanceFromCamera = 20f;
        [SerializeField] private float spawnDistanceOutsideView = 0.05f;
        [SerializeField] private float initialSpawnDelay = 1f;
        [SerializeField] private float minimumSpawnDelay = 0.2f;
        [SerializeField] private float spawnAcceleration = 0.01f;

        [SerializeField] private int maximumItemCountAllowed = 10;
        private int currentItemCount = 0;

        private float currentSpawnDelay;
        private float spawnTimer;
        private Vector3 centerOfScreenPosition;
        private Vector3 randomScreenPosition;

        private enum ScreenSide 
        { 
            Top, 
            Bottom, 
            Left, 
            Right 
        }

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            centerOfScreenPosition = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane + spawnDistanceFromCamera));

            currentSpawnDelay = initialSpawnDelay;
        }

        void Update()
        {
            if (currentItemCount >= maximumItemCountAllowed)
            {
                return;
            }

            spawnTimer += Time.deltaTime;

            if (spawnTimer >= currentSpawnDelay)
            {
                spawnTimer -= spawnTimer;
                currentSpawnDelay = Mathf.Max(minimumSpawnDelay, currentSpawnDelay - spawnAcceleration);
                SpawnItemFromRandomSide();
            }        
        }

        public void ReduceItemCount()
        {
            currentItemCount--;
        }

        private void SpawnItemFromRandomSide()
        {
            currentItemCount++;
            ScreenSide side = (ScreenSide)Random.Range(0, 4);
            Vector3 screenSpawnPosition = GetSpawnPosition(side);
            Vector3 worldSpawnPosition = mainCamera.ViewportToWorldPoint(screenSpawnPosition);

            GameObject item = Instantiate(itemPrefab, worldSpawnPosition, Quaternion.identity);

            //Example of adding code to spawned items - normally just manually add

            ////Use this code - it adds a script to the spawned item that allows it to wrap around the camera frame
            //item.AddComponent<WrapAroundScreen>();

            ////Or, Use this code - the script will move it to the center of the screen
            //item.AddComponent<MoveTowardsCenter>().Initialize(centerOfScreenPosition);

            ////Or, Use this code - the script will move it to a random point on the screen
            //randomScreenPosition = mainCamera.ViewportToWorldPoint(new Vector3(Random.value, Random.value, mainCamera.nearClipPlane + spawnDistanceFromCamera));
            //item.AddComponent<MoveTowardsCenter>().Initialize(randomScreenPosition);
        }

        private Vector3 GetSpawnPosition(ScreenSide side)
        {
            switch (side)
            {
                case ScreenSide.Top: 
                    return new Vector3(Random.value, 1f + spawnDistanceOutsideView, mainCamera.nearClipPlane + spawnDistanceFromCamera);
                case ScreenSide.Bottom: 
                    return new Vector3(Random.value, -spawnDistanceOutsideView, mainCamera.nearClipPlane + spawnDistanceFromCamera);
                case ScreenSide.Left: 
                    return new Vector3(-spawnDistanceOutsideView, Random.value, mainCamera.nearClipPlane + spawnDistanceFromCamera);
                case ScreenSide.Right: 
                    return new Vector3(1f + spawnDistanceOutsideView, Random.value, mainCamera.nearClipPlane + spawnDistanceFromCamera);
                default: 
                    return Vector3.zero;
            }
        }
    }
}
