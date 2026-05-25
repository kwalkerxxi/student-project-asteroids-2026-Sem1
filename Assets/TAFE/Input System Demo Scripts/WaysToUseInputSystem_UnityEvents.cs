using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// This shows using the Input System with 
/// Using Invoke Unity Events
/// You can set up Unity Events in the Inspector.
/// ✔ Pros: Designer - friendly, reusable
/// ❌ Cons: Requires manual setup in the Inspector
/// </summary>

public class WaysToUseInputSystem_UnityEvents : MonoBehaviour
{

    [SerializeField] private PlayerInput controls;

    public Vector3 movementValue;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool excludeY = false;
    [SerializeField] private bool flipScaleMovement = false;

    [SerializeField] MoveType typeOfMovement = MoveType.XZ3D;
    private void OnEnable()
    {
        //.started      (once)
        //.performed    (on going)
        //.canceled     (once)
        controls.actions["Jump"].performed += JumpCode;
        controls.actions["Move"].performed += MoveCode;
        controls.actions["Move"].canceled += MoveCode;
    }

    private void OnDisable()
    {
        controls.actions["Jump"].performed -= JumpCode;
        controls.actions["Move"].performed -= MoveCode;
        controls.actions["Move"].canceled -= MoveCode;
    }

    public void JumpCode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump Code here - Using Unity Events");
            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
            }
        }
    }
    public void MoveCode(InputAction.CallbackContext value)
    {
        movementValue = value.ReadValue<Vector2>();
        if (excludeY)
        {
            movementValue.y = 0;
        }

        if (flipScaleMovement)
        {
            if (movementValue.x < 0)
            {
                transform.localScale = new Vector3(1, 1, -1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void Update()
    {
        if (typeOfMovement == MoveType.XY2D)
        {
            transform.Translate(new Vector3(movementValue.x, movementValue.y, 0) * moveSpeed * Time.deltaTime);
        }
        else if (typeOfMovement == MoveType.XZ3D)
        {
            transform.Translate(new Vector3(movementValue.x, 0, movementValue.y) * moveSpeed * Time.deltaTime);
        }
        else if (typeOfMovement == MoveType.ZY2D)
        {
            transform.Translate(new Vector3(0, movementValue.y, movementValue.x) * moveSpeed * Time.deltaTime);
        }


    }
}

public enum MoveType
{
    XY2D,
    XZ3D,
    ZY2D
}
