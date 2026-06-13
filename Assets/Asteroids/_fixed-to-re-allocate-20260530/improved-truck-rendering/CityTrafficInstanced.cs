using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

namespace Riley.Scripts
{
    public class CityTrafficInstanced : MonoBehaviour
    {
        [SerializeField] private SplineContainer[] truckSplinePaths;

        [Header("Rendering")]
        [SerializeField] private Mesh truckMesh;
        [SerializeField] Material[] truckMaterials;
        [SerializeField] private float truckScale = 1f;

        [Header("Traffic")]
        [SerializeField] private int spawnCount = 10;
        [SerializeField] private float truckSpeed = 5f;

        struct Truck
        {
            public SplineContainer spline;
            public float normalizedPosition;
            public float splineLength;
        }

        private Truck[] trucks;
        private Matrix4x4[] matrices;

        [SerializeField]
        private RenderParams[] submeshParams;

        [SerializeField]
        private int renderLayer = 11;
        [SerializeField] private string renderLayerName = "Buildings";

        void Start()
        {
            if(truckSplinePaths.Length <= 0)
            {
                truckSplinePaths = GetComponentsInChildren<SplineContainer>();
            }


            InitializeTraffic();

            submeshParams = new RenderParams[truckMesh.subMeshCount];

            for(int i = 0; i < truckMesh.subMeshCount; i++)
            {
                submeshParams[i] = new RenderParams(truckMaterials[i])
                {
                    shadowCastingMode = ShadowCastingMode.Off,
                    receiveShadows = false,
                    layer = LayerMask.NameToLayer(renderLayerName)
                };
            }
        }

        void InitializeTraffic()
        {
            int totalTrucks = truckSplinePaths.Length * spawnCount;

            trucks = new Truck[totalTrucks];
            matrices = new Matrix4x4[totalTrucks];

            int index = 0;

            foreach(SplineContainer spline in truckSplinePaths)
            {
                float length = spline.CalculateLength();
                float spacing = length / (spawnCount - 1);

                for(int i = 0; i < spawnCount; i++)
                {
                    float distance = spacing * i;

                    float t =
                        spline.Spline.ConvertIndexUnit(
                            distance,
                            PathIndexUnit.Distance,
                            PathIndexUnit.Normalized);

                    trucks[index] = new Truck
                    {
                        spline = spline,
                        normalizedPosition = t,
                        splineLength = length
                    };

                    index++;
                }
            }
        }

        void Update()
        {
            UpdateTraffic();

            for(int submesh = 0; submesh < truckMesh.subMeshCount; submesh++)
            {
                Graphics.RenderMeshInstanced(
                    submeshParams[submesh],
                    truckMesh,
                    submesh,
                    matrices,
                    matrices.Length
                );
            }
        }



        void UpdateTraffic()
        {
            for(int i = 0; i < trucks.Length; i++)
            {
                Truck truck = trucks[i];

                truck.normalizedPosition +=
                    (truckSpeed / truck.splineLength) *
                    Time.deltaTime;

                truck.normalizedPosition %= 1f;

                Vector3 position =
                    truck.spline.EvaluatePosition(
                        truck.normalizedPosition);

                Vector3 tangent =
                    truck.spline.EvaluateTangent(
                        truck.normalizedPosition);

                Quaternion rotation =
                    tangent.sqrMagnitude > 0.0001f
                        ? Quaternion.LookRotation(tangent)
                        : Quaternion.identity;


                matrices[i] = Matrix4x4.TRS(
                    position,
                    rotation,
                    Vector3.one * truckScale);

                trucks[i] = truck;
            }
        }
    }
}