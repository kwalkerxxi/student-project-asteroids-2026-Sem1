using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public enum SpawnQuadrantEdge
{
    LeftTop,
    TopRight,
    RightBottom,
    BottomLeft
}

public class LaneSpawner : MonoBehaviour
{
    List<Vector3> lanes = new List<Vector3>();

    [SerializeField]
    private int amountOfShipsToSpawn = 5;



    [Header("Lane Counts")]
    public int leftTopLanes = 3;
    public int topRightLanes = 3;
    public int rightBottomLanes = 3;
    public int bottomLeftLanes = 3;

    [Header("Spawn Settings")]
    public float spawnOffset = 1f;
    public GameObject prefab;

    private Camera cam;

    [SerializeField]
    private bool showSpawnLocations = false;

    void Awake()
    {
        cam = Camera.main;
    }

    // ------------------------------------------------
    // Demo spawning
    // ------------------------------------------------
    void Start()
    {
        Invoke(nameof(InitialiseSpawning), 0.5f);
    }


    struct GroundFrustum
    {
        public Vector3 BL;
        public Vector3 TL;
        public Vector3 TR;
        public Vector3 BR;
    }

    //GroundFrustum GetGroundFrustum()
    //{
    //    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

    //    return new GroundFrustum
    //    {
    //        BL = ViewportToGroundPoint(new Vector2(0, 0), groundPlane),
    //        TL = ViewportToGroundPoint(new Vector2(0, 1), groundPlane),
    //        TR = ViewportToGroundPoint(new Vector2(1, 1), groundPlane),
    //        BR = ViewportToGroundPoint(new Vector2(1, 0), groundPlane),
    //    };
    //}

    GroundFrustum GetGroundFrustum(float margin = 0f)
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        Vector2 center = new Vector2(0.5f, 0.5f);
        float halfSize = 0.5f * (1f + margin * 2f);

        Vector2 bl = center + new Vector2(-halfSize, -halfSize);
        Vector2 tl = center + new Vector2(-halfSize, halfSize);
        Vector2 tr = center + new Vector2(halfSize, halfSize);
        Vector2 br = center + new Vector2(halfSize, -halfSize);

        return new GroundFrustum
        {
            BL = ViewportToGroundPoint(bl, groundPlane),
            TL = ViewportToGroundPoint(tl, groundPlane),
            TR = ViewportToGroundPoint(tr, groundPlane),
            BR = ViewportToGroundPoint(br, groundPlane),
        };
    }

    GroundFrustum GetGroundFrustum(float marginX = 0f, float marginY = 0f)
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float minX = 0f - marginX;
        float maxX = 1f + marginX;

        float minY = 0f - marginY;
        float maxY = 1f + marginY;

        return new GroundFrustum
        {
            BL = ViewportToGroundPoint(new Vector2(minX, minY), groundPlane),
            TL = ViewportToGroundPoint(new Vector2(minX, maxY), groundPlane),
            TR = ViewportToGroundPoint(new Vector2(maxX, maxY), groundPlane),
            BR = ViewportToGroundPoint(new Vector2(maxX, minY), groundPlane),
        };
    }

    // ------------------------------------------------
    // Get visible XZ bounds from top-down camera
    // ------------------------------------------------
    void GetBounds(out float minX, out float maxX,
                  out float minZ, out float maxZ)
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        Vector3[] corners =
        {
        ViewportToGroundPoint(new Vector2(0, 0), groundPlane), // BL
        ViewportToGroundPoint(new Vector2(0, 1), groundPlane), // TL
        ViewportToGroundPoint(new Vector2(1, 0), groundPlane), // BR
        ViewportToGroundPoint(new Vector2(1, 1), groundPlane), // TR
    };

        minX = float.MaxValue;
        maxX = float.MinValue;
        minZ = float.MaxValue;
        maxZ = float.MinValue;

        foreach(var p in corners)
        {
            minX = Mathf.Min(minX, p.x);
            maxX = Mathf.Max(maxX, p.x);

            minZ = Mathf.Min(minZ, p.z);
            maxZ = Mathf.Max(maxZ, p.z);
        }

        if(!showSpawnLocations)
        {
            return;
        }

        foreach(var p in corners)
        {
            CreatePrimitiveWithoutCollider(
                PrimitiveType.Capsule,
                p,
                Vector3.one * 3);
        }
    }

    Vector3 ViewportToGroundPoint(Vector2 viewportPoint, Plane groundPlane)
    {
        Ray ray = cam.ViewportPointToRay(viewportPoint);

        if(groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }


    public GameObject CreatePrimitiveWithoutCollider(PrimitiveType primitiveType, Vector3 position, Vector3 scale)
    {


        GameObject primitiveObject = GameObject.CreatePrimitive(primitiveType);

        if(primitiveObject.TryGetComponent<Collider>(out Collider collider))
        {
            Object.Destroy(collider);
        }

        primitiveObject.transform.position = position;
        primitiveObject.transform.localScale = scale;

        return primitiveObject;
    }


    // ------------------------------------------------
    // Build lanes for a quadrant-edge
    // ------------------------------------------------
    List<Vector3> GetLanePositions(SpawnQuadrantEdge edge, int laneCount)
    {
        GroundFrustum f = GetGroundFrustum(0.03f, 0.03f);

        List<Vector3> lanes = new();

        switch(edge)
        {
            case SpawnQuadrantEdge.LeftTop:
                {
                    Vector3 start = Vector3.Lerp(f.BL, f.TL, 0.5f);
                    Vector3 end = f.TL;

                    for(int i = 0; i < laneCount; i++)
                    {
                        float t = (i + 0.5f) / laneCount;
                        Vector3 p = Vector3.Lerp(start, end, t);

                        Vector3 outward =
                            Vector3.Cross(Vector3.up, (f.TL - f.BL).normalized);

                        lanes.Add(p + outward * spawnOffset);
                    }
                    break;
                }

            case SpawnQuadrantEdge.TopRight:
                {
                    Vector3 start = Vector3.Lerp(f.TL, f.TR, 0.5f);
                    Vector3 end = f.TR;

                    for(int i = 0; i < laneCount; i++)
                    {
                        float t = (i + 0.5f) / laneCount;
                        Vector3 p = Vector3.Lerp(start, end, t);

                        Vector3 outward =
                            Vector3.Cross(Vector3.up, (f.TR - f.TL).normalized);

                        lanes.Add(p + outward * spawnOffset);
                    }
                    break;
                }

            case SpawnQuadrantEdge.RightBottom:
                {
                    Vector3 start = f.BR;
                    Vector3 end = Vector3.Lerp(f.BR, f.TR, 0.5f);

                    for(int i = 0; i < laneCount; i++)
                    {
                        float t = (i + 0.5f) / laneCount;
                        Vector3 p = Vector3.Lerp(start, end, t);

                        Vector3 outward =
                            Vector3.Cross(Vector3.up, (f.BR - f.TR).normalized);

                        lanes.Add(p + outward * spawnOffset);
                    }
                    break;
                }

            case SpawnQuadrantEdge.BottomLeft:
                {
                    Vector3 start = f.BL;
                    Vector3 end = Vector3.Lerp(f.BL, f.BR, 0.5f);

                    for(int i = 0; i < laneCount; i++)
                    {
                        float t = (i + 0.5f) / laneCount;
                        Vector3 p = Vector3.Lerp(start, end, t);

                        Vector3 outward =
                            Vector3.Cross(Vector3.up, (f.BL - f.BR).normalized);

                        lanes.Add(p + outward * spawnOffset);
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



    void InitialiseSpawning()
    {
        //InvokeRepeating(nameof(SpawnRandom), 1f, 2f);

        SpawnSpots();

        ShuffleUtils.FisherYatesShuffle(lanes);

        //IF STAND ALONE USE THESE LINES
        // lanes = lanes.Take(Mathf.Min(amountOfShipsToSpawn, lanes.Count())).ToList();
        //SpawnSequentialIntoScreen(0.1f);

        //FOR NON-SEQUENTIAL SPAWNING DO THIS
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


        if(!showSpawnLocations)
        {
            return;
        }

        foreach(var lane in lanes)
        {
            GameObject spawnSpot = CreatePrimitiveWithoutCollider(PrimitiveType.Cube, lane, Vector3.one);

            spawnSpot.transform.SetParent(spawnLocations);

            //Destroy(spawnSpot.GetComponent<Collider>());
        }
    }


    public void SpawnSequentialIntoScreen(float delay = 0.2f)
    {
        StartCoroutine(SpawnSequentialIntoScreenRoutine(delay));
    }

    //private IEnumerator SpawnSequentialIntoScreenRoutine(float delay)
    //{
    //    // Get screen bounds once
    //    GetBounds(out float minX, out float maxX, out float minZ, out float maxZ);

    //    foreach(var pos in lanes)
    //    {
    //        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
    //        obj.transform.localScale *= 1f;

    //        WrapCollisionFixer.LargeAsteroids.Add(obj.transform);

    //        // Determine which side of the screen this lane is on
    //        float distLeft = Mathf.Abs(pos.x - minX);
    //        float distRight = Mathf.Abs(pos.x - maxX);
    //        float distBottom = Mathf.Abs(pos.z - minZ);
    //        float distTop = Mathf.Abs(pos.z - maxZ);

    //        // Pick the closest edge → rotate inward
    //        float minDist = Mathf.Min(distLeft, distRight, distBottom, distTop);

    //        if(minDist == distLeft)
    //        {
    //            // Left side → face right
    //            obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    //        }
    //        else if(minDist == distRight)
    //        {
    //            // Right side → face left
    //            obj.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
    //        }
    //        else if(minDist == distTop)
    //        {
    //            // Top → face down
    //            obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    //        }
    //        else
    //        {
    //            // Bottom → face up
    //            obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    //        }

    //        yield return new WaitForSeconds(delay);
    //    }
    //}


    private IEnumerator SpawnSequentialIntoScreenRoutine(float delay)
    {
        GetBounds(out float minX, out float maxX, out float minZ, out float maxZ);

        foreach(var pos in lanes)
        {
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.transform.localScale *= 1f;

            WrapCollisionFixer.LargeAsteroids.Add(obj.transform);


            var side = ScreenBoundsChecker.GetOffScreenSide(pos, Camera.main);

            if(side == ScreenBoundsChecker.OffScreenSide.Left)
            {
                obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
            else if(side == ScreenBoundsChecker.OffScreenSide.Right)
            {
                obj.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }
            else if(side == ScreenBoundsChecker.OffScreenSide.Top)
            {
                obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            yield return new WaitForSeconds(delay);
        }
    }


    public GameObject SpawnAtSetSideLocationIndex(int index)
    {
        if(index < 0 || index >= lanes.Count)
        {
            return null;
        }

        GetBounds(out float minX, out float maxX, out float minZ, out float maxZ);

        Vector3 pos = lanes[index];

        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.transform.localScale *= 1f;

        WrapCollisionFixer.LargeAsteroids.Add(obj.transform);

        var side = ScreenBoundsChecker.GetOffScreenSide(pos, Camera.main);

        if(side == ScreenBoundsChecker.OffScreenSide.Left)
        {
            obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if(side == ScreenBoundsChecker.OffScreenSide.Right)
        {
            obj.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        }
        else if(side == ScreenBoundsChecker.OffScreenSide.Top)
        {
            obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        return obj;
    }

}