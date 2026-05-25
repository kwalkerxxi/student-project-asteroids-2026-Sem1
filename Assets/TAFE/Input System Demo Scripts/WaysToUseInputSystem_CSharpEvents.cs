using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This shows using the Input System with 
/// Using Invoke C Sharp (C#) Events
/// You can set up Unity Events in the Inspector.
/// ✔ Pros: High performance, type-safe
/// ❌ Cons: Requires managing event subscriptions manually
/// </summary>
public class WaysToUseInputSystem_CSharpEvents : MonoBehaviour
{
    // Instance of the input action c# script that is automatically created
    // when generate c# class checkbox is enabled
    // The class name may be different if you manually change it
    // NOTE: No built-in support for local multi-player!
    InputSystem_Actions inputSystem;

    //Variables for this simple demo to work
    public Vector3 movementValue;
    [SerializeField] private float moveSpeed = 5f;
    private void Start()
    {
        inputSystem = new InputSystem_Actions();
        inputSystem.Enable();

        //If you noticed the order of access,
        //its from Input actions class -> Action Maps -> Actions

        inputSystem.Player.Jump.performed += JumpCode;

        inputSystem.Player.Move.performed += MoveCode;
        inputSystem.Player.Move.canceled += MoveCode;
    }

    private void Update()
    {
        transform.Translate(new Vector3(movementValue.x, 0, movementValue.y) * moveSpeed * Time.deltaTime);
    }

    public void MoveCode(InputAction.CallbackContext value)
    {
        movementValue = value.ReadValue<Vector2>();
    }

    public void JumpCode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump Code here - Using C# Actions");
        }
    }
}
