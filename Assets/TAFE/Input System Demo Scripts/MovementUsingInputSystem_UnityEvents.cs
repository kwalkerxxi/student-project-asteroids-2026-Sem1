using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This shows using the Input System with 
/// Using Invoke Unity Events
/// You can set up Unity Events in the Inspector.
/// ✔ Pros: Designer - friendly, reusable
/// ❌ Cons: Requires manual setup in the Inspector (if not wanting to use code version below)
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class MovementUsingInputSystem_UnityEvents : MonoBehaviour
{
    // Could require user to manually add a PlayerInput component to the same GameObject as this script
    // However, in this example this is not required as the RequireComponent attribute is used
    [SerializeField] private PlayerInput controls;

    private InputAction jumpInput;
    private InputAction moveInput;
    private InputAction lookInput;
    //InputActions have some common states
    //.started      (once)
    //.performed    (on going)
    //.canceled     (once)

    private Vector3 movementValue;
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 lookValue;
    [SerializeField] private float lookSpeed = 15f;

    // Use either the Transform or Rigidbody for movement
    // NOT BOTH!
    private Transform cachedTransform;
    private Rigidbody cachedRigidbody;

    [SerializeField] private Camera cameraForRelativeMovement;
    private void Awake()
    {
        // Cache a reference to the PlayerInput component  
        controls = GetComponent<PlayerInput>();

        // Cache references to input actions for easier use in script
        jumpInput = controls.actions["Jump"];
        moveInput = controls.actions["Move"];
        lookInput = controls.actions["Look"];

        // Cache components for better memory management
        cachedTransform = GetComponent<Transform>();
        cachedRigidbody = GetComponent<Rigidbody>();

        if (cameraForRelativeMovement == null)
        {
            cameraForRelativeMovement = Camera.main;
        }
    }

    // Automatically assign methods to input actions
    // This can be manually done in the inspector if you prefer
    private void OnEnable()
    {
        jumpInput.performed += JumpInputCode;

        moveInput.performed += MoveInputCode;
        moveInput.canceled += MoveInputCode;

        lookInput.performed += LookInputCode;
        lookInput.canceled += LookInputCode;
    }

    // Automatically unassign methods to input actions
    private void OnDisable()
    {
        jumpInput.performed -= JumpInputCode;

        moveInput.performed -= MoveInputCode;
        moveInput.canceled -= MoveInputCode;

        lookInput.performed -= LookInputCode;
        lookInput.canceled -= LookInputCode;
    }

    public void JumpInputCode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump Code here - Using Unity Events");
            if (cachedRigidbody == null) { return; }
            cachedRigidbody.AddRelativeForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }
    public void MoveInputCode(InputAction.CallbackContext value)
    {
        movementValue = value.ReadValue<Vector2>();
    }

    public void LookInputCode(InputAction.CallbackContext value)
    {
        lookValue = value.ReadValue<Vector2>();
    }

    // Use Update when working with the Transform
    private void Update()
    {
        //// Relative to Player GameObject Movement
        //cachedTransform.Translate(new Vector3(movementValue.x, 0, movementValue.y) * moveSpeed * Time.deltaTime);
        //// Relative to Player GameObject Rotation
        //cachedTransform.Rotate(new Vector3(0, lookValue.x, 0) * lookSpeed * Time.deltaTime);

        // OR
        // Relative to Camera Movement
        // with automatic rotation
        //if (cameraForRelativeMovement != null)
        //{
        //    // Get the camera's forward direction but ignore any vertical tilt (lock Y)
        //    Vector3 camForward = cameraForRelativeMovement.transform.forward;
        //    Vector3 camRight = cameraForRelativeMovement.transform.right;

        //    camForward.y = 0; // Lock Y movement
        //    camRight.y = 0;   // Lock Y movement

        //    camForward.Normalize();
        //    camRight.Normalize();

        //    // Move relative to the camera's orientation but locked to the X-Z plane
        //    Vector3 moveDirection = (camRight * movementValue.x + camForward * movementValue.y).normalized;

        //    // Apply movement
        //    cachedTransform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        //    // Rotate towards moving direction
        //    if (moveDirection.magnitude > 0)
        //    {
        //        cachedTransform.rotation = Quaternion.LookRotation(moveDirection);
        //    }
        //}
    }

    // Use FixedUpdate when working with the Rigidbody
    private void FixedUpdate()
    {
        if(cachedRigidbody== null) {  return; }

        //// Relative to Player GameObject Movement
        //// Relative to Player GameObject Movement
        //cachedRigidbody.AddRelativeForce(new Vector3(movementValue.x, 0, movementValue.y) * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        //// Relative to Player GameObject Rotation
        //cachedRigidbody.AddRelativeTorque(new Vector3(0, lookValue.x, 0) * lookSpeed * Time.deltaTime, ForceMode.VelocityChange);

        // OR
        // Relative to Camera Movement
        // with automatic rotation
        if (cameraForRelativeMovement != null)
        {
            // Get the camera's forward direction but ignore any vertical tilt (lock Y)
            Vector3 camForward = cameraForRelativeMovement.transform.forward;
            Vector3 camRight = cameraForRelativeMovement.transform.right;

            camForward.y = 0; // Lock Y movement
            camRight.y = 0;   // Lock Y movement

            camForward.Normalize();
            camRight.Normalize();

            // Move relative to the camera's orientation but locked to the X-Z plane
            Vector3 moveDirection = (camRight * movementValue.x + camForward * movementValue.y).normalized;

            // Apply movement
            cachedRigidbody.AddForce(moveDirection * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            // Rotate towards moving direction
            if (moveDirection.magnitude > 0)
            {
                cachedTransform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }
}
