using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This shows using the Input System with 
/// Using an Input Action Map
/// ✔ Pros: Supports multiple devices, easy rebinding
/// ❌ Cons: Requires setting up an Input Action asset
/// </summary>
public class WaysToUseInputSystem_InputAction : MonoBehaviour
{
    [SerializeField] InputAction jumpAction;

    //Input Action Example
    void Awake()
    {
        //// CODED SETUP
        //// Create a new input action
        //jumpAction = new InputAction("Jump", InputActionType.Button);

        //// Bind it to the spacebar and gamepad A button
        //jumpAction.AddBinding("<Keyboard>/space");
        //jumpAction.AddBinding("<Gamepad>/buttonSouth"); // A on Xbox, X on PlayStation

        // Subscribe to the event
        jumpAction.performed += OnJump;
    }

    //private void OnEnable()
    //{
    //    jumpAction.Enable();
    //}

    //private void OnDisable()
    //{
    //    jumpAction.Disable();
    //}

    private void OnEnable() => jumpAction.Enable();
    private void OnDisable() => jumpAction.Disable();


    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump action performed using InputAction!");
    }


}
