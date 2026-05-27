using UnityEngine;

public class RotateObjectsRandomized : MonoBehaviour
{
    [System.Serializable]
    private class RotationTarget
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private float minimumRotationSpeed = 10f;
        [SerializeField] private float maximumRotationSpeed = 50f;

        private float actualRotationSpeed;
        private float rotationDirection = 1f;

        public GameObject TargetObject => targetObject;
        public float ActualRotationSpeed => actualRotationSpeed;
        public float RotationDirection => rotationDirection;

        public void InitializeRandomSpeed()
        {
            actualRotationSpeed = Random.Range(minimumRotationSpeed, maximumRotationSpeed);
        }

        public void ReverseDirection()
        {
            rotationDirection *= -1f;
        }
    }

    [SerializeField] private RotationTarget[] rotationTargets;

    [SerializeField] private bool enablePingPong = false;
    [SerializeField] private float pingPongIntervalSeconds = 2f;

    private float pingPongTimerSeconds = 0f;

    private void Start()
    {
        foreach(RotationTarget rotationTarget in rotationTargets)
        {
            if(rotationTarget.TargetObject != null)
            {
                rotationTarget.InitializeRandomSpeed();
            }
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if(enablePingPong)
        {
            pingPongTimerSeconds += deltaTime;

            if(pingPongTimerSeconds >= pingPongIntervalSeconds)
            {
                pingPongTimerSeconds = 0f;

                foreach(RotationTarget rotationTarget in rotationTargets)
                {
                    rotationTarget.ReverseDirection();
                }
            }
        }

        foreach(RotationTarget rotationTarget in rotationTargets)
        {
            if(rotationTarget.TargetObject != null)
            {
                rotationTarget.TargetObject.transform.Rotate(
                    0f,
                    rotationTarget.ActualRotationSpeed *
                    rotationTarget.RotationDirection *
                    deltaTime,
                    0f,
                    Space.World
                );
            }
        }
    }
}
