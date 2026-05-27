using UnityEngine;
using UnityEngine.InputSystem;

public class RotateAndScaleObjectsViaControls : MonoBehaviour
{
    [SerializeField] private InputAction movementInputAction;
    [SerializeField] private InputAction zAxisRotationInputAction;

    [SerializeField] private GameObject[] controlledObjects;

    [SerializeField] private float yAxisRotationSpeedDegreesPerSecond = 90f;
    [SerializeField] private float zAxisRotationSpeedDegreesPerSecond = 90f;
    [SerializeField] private float minimumScale = 1f;
    [SerializeField] private float maximumScale = 2f;
    [SerializeField] private float scaleSpeedPerSecond = 1f;

    private Vector2 movementInputValue;
    private float zAxisRotationInputValue;

    [Header("Optional Override of Automatic Rotating Script")]
    [SerializeField] private RotateObjectsRandomized autoObjectRotator;

    private void OnEnable()
    {
        movementInputAction.Enable();
        zAxisRotationInputAction.Enable();

        movementInputAction.performed += OnMovementInputPerformed;
        movementInputAction.canceled += OnMovementInputCanceled;

        zAxisRotationInputAction.performed += OnZAxisRotationPerformed;
        zAxisRotationInputAction.canceled += OnZAxisRotationCanceled;
    }

    private void OnDisable()
    {
        movementInputAction.performed -= OnMovementInputPerformed;
        movementInputAction.canceled -= OnMovementInputCanceled;

        zAxisRotationInputAction.performed -= OnZAxisRotationPerformed;
        zAxisRotationInputAction.canceled -= OnZAxisRotationCanceled;


        movementInputAction.Disable();
        zAxisRotationInputAction.Disable();
    }

    private void OnMovementInputPerformed(InputAction.CallbackContext context)
    {
        movementInputValue = context.ReadValue<Vector2>();

        if(autoObjectRotator != null)
        {
            autoObjectRotator.enabled = false;
        }
    }

    private void OnMovementInputCanceled(InputAction.CallbackContext context)
    {
        movementInputValue = Vector2.zero;

        if(autoObjectRotator != null)
        {
            autoObjectRotator.enabled = true;
        }
    }

    private void OnZAxisRotationPerformed(InputAction.CallbackContext context)
    {
        zAxisRotationInputValue = context.ReadValue<float>();
    }

    private void OnZAxisRotationCanceled(InputAction.CallbackContext context)
    {
        zAxisRotationInputValue = 0f;
    }


    private void Update()
    {
        float deltaTime = Time.deltaTime;

        RotateOnYAxis(movementInputValue.x, deltaTime);
        ScaleObjects(movementInputValue.y, deltaTime);
        RotateOnZAxis(zAxisRotationInputValue, deltaTime);
    }

    private void RotateOnYAxis(float horizontalInput, float deltaTime)
    {
        if(Mathf.Approximately(horizontalInput, 0f))
        {
            return;
        }

        float rotationAmount =
            horizontalInput *
            yAxisRotationSpeedDegreesPerSecond *
            deltaTime;

        foreach(GameObject controlledObject in controlledObjects)
        {
            if(controlledObject != null)
            {
                controlledObject.transform.Rotate(
                    0f,
                    rotationAmount,
                    0f,
                    Space.World
                );
            }
        }
    }

    private void RotateOnZAxis(float zInput, float deltaTime)
    {
        if(Mathf.Approximately(zInput, 0f))
        {
            return;
        }

        float rotationAmount =
            zInput *
            zAxisRotationSpeedDegreesPerSecond *
            deltaTime;

        foreach(GameObject controlledObject in controlledObjects)
        {
            if(controlledObject != null)
            {
                controlledObject.transform.Rotate(
                    0f,
                    0f,
                    rotationAmount,
                    Space.Self
                );
            }
        }
    }

    private void ScaleObjects(float verticalInput, float deltaTime)
    {
        if(Mathf.Approximately(verticalInput, 0f))
        {
            return;
        }

        foreach(GameObject controlledObject in controlledObjects)
        {
            if(controlledObject != null)
            {
                Vector3 currentScale = controlledObject.transform.localScale;

                float scaleChange =
                    verticalInput *
                    scaleSpeedPerSecond *
                    deltaTime;

                float targetScale =
                    Mathf.Clamp(
                        currentScale.x + scaleChange,
                        minimumScale,
                        maximumScale
                    );

                controlledObject.transform.localScale =
                    new Vector3(targetScale, targetScale, targetScale);
            }
        }
    }
}

