using UnityEngine;
using UnityEngine.Splines;

namespace Riley.Scripts
{

    ///<summary>
    ///This script is to spawn an amount of objects along a spline.
    ///</summary>

    public class CityTrafficSpawn : MonoBehaviour
    {

        [SerializeField] SplineContainer[] truckSplinePaths;
        [SerializeField] GameObject truckPrefab;
        [SerializeField] int spawnCount = 10;

        Transform truckHolder;

        public float truckPosition;

        private void Start()
        {
            truckHolder = new GameObject("City Truck Holder").transform;
            Spawn();
        }

        void Spawn()
        {
            if (truckSplinePaths != null)
            {
                foreach (SplineContainer splinePath in truckSplinePaths)
                {
                    float length = splinePath.CalculateLength();
                    float spacing = length / (spawnCount - 1);

                    for (int i = 0; i < spawnCount; i++)
                    {
                        //calculating even spacing between objects                 
                        float distance = spacing * i;
                        truckPosition = splinePath.Spline.ConvertIndexUnit(distance, PathIndexUnit.Distance, PathIndexUnit.Normalized);

                        Vector3 position = splinePath.EvaluatePosition(truckPosition);
                        GameObject truck = Instantiate(truckPrefab, position, Quaternion.identity, truckHolder);

                        if (truck.TryGetComponent<SplineFollower>(out SplineFollower splineFollower))
                        {
                            //insure that the objects and still evenly spaced when moving with splinefollower script                    
                            splineFollower.truckPath = splinePath;
                            splineFollower.distanceTravelled = truckPosition;
                        }
                        else
                        {
                            Debug.Log("No SplineFollower Script Can Be Found");
                        }
                    }
                }

            }
            else
            {
                Debug.Log("Missing SplinePath In Array, Need To Assign");
            }
        }
    }
}