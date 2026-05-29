using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GamepadMouseClick : MonoBehaviour
{
    public InputActionReference gamepadClickAction; // Assign this in the Inspector
    private Mouse virtualMouse;

    private void Awake()
    {
        virtualMouse = InputSystem.GetDevice<Mouse>();
    }

    private void OnEnable()
    {
        gamepadClickAction.action.performed += OnGamepadPress;
        gamepadClickAction.action.canceled += OnGamepadRelease;
        gamepadClickAction.action.Enable();
    }

    private void OnDisable()
    {
        gamepadClickAction.action.performed -= OnGamepadPress;
        gamepadClickAction.action.canceled -= OnGamepadRelease;
        gamepadClickAction.action.Disable();
    }

    private void OnGamepadPress(InputAction.CallbackContext context)
    {
        if (virtualMouse != null)
        {
            ////InputState.Change(virtualMouse.leftButton, 1); // Release left mouse button

            //var mouseState = new MouseState { buttons = 1 }; // Left mouse button down
            //InputSystem.QueueStateEvent(virtualMouse, mouseState);
            //InputSystem.Update(); // Ensure the change is processed immediately


        }
    }

    private void OnGamepadRelease(InputAction.CallbackContext context)
    {
        if (virtualMouse != null)
        {
            ////InputState.Change(virtualMouse.leftButton, 0); // Release left mouse button

            //var mouseState = new MouseState { buttons = 0 }; // Release all buttons
            //InputSystem.QueueStateEvent(virtualMouse, mouseState);
            //InputSystem.Update(); // Process immediately
        }
    }
}
