using UnityEngine;
using UnityEngine.Splines;

namespace Riley.Scripts
{

    ///<summary>
    ///This script is to make an object follow a spline at a set speed.
    ///</summary>

    public class SplineFollower : MonoBehaviour
    {

        public SplineContainer truckPath;
        [SerializeField] float truckSpeed = 5f;
        [Range(0f, 1f)] public float distanceTravelled;

        private float splineLength;

        void Start()
        {
            splineLength = truckPath.CalculateLength();
        }

        void Update()
        {
            if (truckPath == null)
            {
                return;
            }

            // Move along spline
            distanceTravelled += (truckSpeed / splineLength) * Time.deltaTime;
            distanceTravelled %= 1f;

            Vector3 position = truckPath.EvaluatePosition(distanceTravelled);
            Vector3 tangent = truckPath.EvaluateTangent(distanceTravelled);

            transform.position = position;

            if (tangent != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(tangent);
            }
        }
    }
}
