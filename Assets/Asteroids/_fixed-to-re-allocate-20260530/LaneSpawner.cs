using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneSpawner : MonoBehaviour
{
    List<Vector3> lanes = new List<Vector3>();

    [SerializeField]
    private int amountOfShipsToSpawn = 5;

    public enum SpawnQuadrantEdge
    {
        LeftTop,
        TopRight,
        RightBottom,
        BottomLeft
    }

    [Header("Lane Counts")]
    public int leftTopLanes = 3;
    public int topRightLanes = 3;
    public int rightBottomLanes = 3;
    public int bottomLeftLanes = 3;

    [Header("Spawn Settings")]
    public float spawnOffset = 1f;
    public GameObject prefab;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    // ------------------------------------------------
    // Get visible XZ bounds from top-down camera
    // ------------------------------------------------
    void GetBounds(out float minX, out float maxX,
                   out float minZ, out float maxZ)
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        Ray rayBL = cam.ScreenPointToRay(Vector3.zero);
        Ray rayTR = cam.ScreenPointToRay(
            new Vector3(Screen.width, Screen.height, 0));

        groundPlane.Raycast(rayBL, out float enterBL);
        groundPlane.Raycast(rayTR, out float enterTR);

        Vector3 bottomLeft = rayBL.GetPoint(enterBL);
        Vector3 topRight = rayTR.GetPoint(enterTR);

        minX = bottomLeft.x;
        maxX = topRight.x;

        minZ = bottomLeft.z;
        maxZ = topRight.z;
    }

    // ------------------------------------------------
    // Build lanes for a quadrant-edge
    // ------------------------------------------------
    List<Vector3> GetLanePositions(SpawnQuadrantEdge edge, int laneCount)
    {
        GetBounds(out float minX, out float maxX,
                  out float minZ, out float maxZ);

        List<Vector3> lanes = new();

        switch(edge)
        {
            case SpawnQuadrantEdge.LeftTop:
                {
                    float startZ = Mathf.Lerp(minZ, maxZ, 0.5f);

                    for(int i = 0; i < laneCount; i++)
                    {
                        float t = (i + 0.5f) / laneCount;
                        float z = Mathf.Lerp(startZ, maxZ, t);

                        lanes.Add(new Vector3(
                            minX - spawnOffset,
                            0f,
                            z));
                    }
                    break;
                }

            case SpawnQuadrantEdge.TopRight:
                {
                    float startX = Mathf.Lerp(minX, maxX, 0.5f);

                    for(int i = 0; i < laneCount; i++)
                    {
                        float t = (i + 0.5f) / laneCount;
                        float x = Mathf.Lerp(startX, maxX, t);

                        lanes.Add(new Vector3(
                            x,
                            0f,
                            maxZ + spawnOffset));
                    }
                    break;
                }

            case SpawnQuadrantEdge.RightBottom:
                {
                    float endZ = Mathf.Lerp(minZ, maxZ, 0.5f);

                    for(int i = 0; i < laneCount; i++)
                    {
                        float t = (i + 0.5f) / laneCount;
                        float z = Mathf.Lerp(minZ, endZ, t);

                        lanes.Add(new Vector3(
                            maxX + spawnOffset,
                            0f,
                            z));
                    }
                    break;
                }

            case SpawnQuadrantEdge.BottomLeft:
                {
                    float endX = Mathf.Lerp(minX, maxX, 0.5f);

                    for(int i = 0; i < laneCount; i++)
                    {
                        float t = (i + 0.5f) / laneCount;
                        float x = Mathf.Lerp(minX, endX, t);

                        lanes.Add(new Vector3(
                            x,
                            0f,
                            minZ - spawnOffset));
                    }
                    break;
                }
        }

        return lanes;
    }



    // ------------------------------------------------
    // Spawn from a specific quadrant-edge
    // ------------------------------------------------
    public void Spawn(SpawnQuadrantEdge edge)
    {
        int laneCount = edge switch
        {
            SpawnQuadrantEdge.LeftTop => leftTopLanes,
            SpawnQuadrantEdge.TopRight => topRightLanes,
            SpawnQuadrantEdge.RightBottom => rightBottomLanes,
            SpawnQuadrantEdge.BottomLeft => bottomLeftLanes,
            _ => 1
        };

        List<Vector3> lanes = GetLanePositions(edge, laneCount);

        if(lanes.Count == 0)
        {
            return;
        }

        Vector3 pos = lanes[Random.Range(0, lanes.Count)];

        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

        obj.transform.localScale *= 0.5f;

        // Face toward world center
        //Vector3 dir = -pos;
        //dir.y = 0f;

        //float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        //obj.transform.rotation = Quaternion.Euler(0f, angle, 0f);

        switch(edge)
        {
            case SpawnQuadrantEdge.LeftTop:
                obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);   // face right
                break;

            case SpawnQuadrantEdge.TopRight:
                obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);  // face down
                break;

            case SpawnQuadrantEdge.RightBottom:
                obj.transform.rotation = Quaternion.Euler(0f, 270f, 0f);  // face left
                break;

            case SpawnQuadrantEdge.BottomLeft:
                obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);    // face up
                break;
        }
    }

    // ------------------------------------------------
    // Demo spawning
    // ------------------------------------------------
    void Start()
    {
        Invoke(nameof(InitialiseSpawning), 1);
    }

    void InitialiseSpawning()
    {
        //InvokeRepeating(nameof(SpawnRandom), 1f, 2f);

        SpawnSpots();

        ShuffleUtils.FisherYatesShuffle(lanes);

        lanes = lanes.Take(Mathf.Min(amountOfShipsToSpawn, lanes.Count())).ToList();
        SpawnSequentialIntoScreen(0.5f);

        //for(int i = 0; i < 400; i++)
        //{
        //    SpawnRandom();
        //}
    }

    void SpawnRandom()
    {
        SpawnQuadrantEdge edge =
            (SpawnQuadrantEdge)Random.Range(0, 4);

        Spawn(edge);
    }

    private void SpawnSpots()
    {

        Transform spawnLocations = new GameObject("Spawn Locations").transform;

        lanes.AddRange(GetLanePositions(SpawnQuadrantEdge.LeftTop, leftTopLanes));
        lanes.AddRange(GetLanePositions(SpawnQuadrantEdge.TopRight, topRightLanes));
        lanes.AddRange(GetLanePositions(SpawnQuadrantEdge.RightBottom, rightBottomLanes));
        lanes.AddRange(GetLanePositions(SpawnQuadrantEdge.BottomLeft, bottomLeftLanes));

        foreach(var lane in lanes)
        {
            GameObject spawnSpot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spawnSpot.transform.position = lane;
            spawnSpot.transform.SetParent(spawnLocations);

            Destroy(spawnSpot.GetComponent<Collider>());
        }
    }


    public void SpawnSequentialIntoScreen(float delay = 0.2f)
    {
        StartCoroutine(SpawnSequentialIntoScreenRoutine(delay));
    }

    private IEnumerator SpawnSequentialIntoScreenRoutine(float delay)
    {
        // Get screen bounds once
        GetBounds(out float minX, out float maxX, out float minZ, out float maxZ);

        foreach(var pos in lanes)
        {
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.transform.localScale *= 1f;

            WrapCollisionFixer.LargeAsteroids.Add(obj.transform);

            // Determine which side of the screen this lane is on
            float distLeft = Mathf.Abs(pos.x - minX);
            float distRight = Mathf.Abs(pos.x - maxX);
            float distBottom = Mathf.Abs(pos.z - minZ);
            float distTop = Mathf.Abs(pos.z - maxZ);

            // Pick the closest edge → rotate inward
            float minDist = Mathf.Min(distLeft, distRight, distBottom, distTop);

            if(minDist == distLeft)
            {
                // Left side → face right
                obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
            else if(minDist == distRight)
            {
                // Right side → face left
                obj.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }
            else if(minDist == distTop)
            {
                // Top → face down
                obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                // Bottom → face up
                obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            yield return new WaitForSeconds(delay);
        }
    }

}