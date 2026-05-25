using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This shows using the Input System with 
/// Direct Calls to Hardware (Polling)
/// ✔ Pros: No setup required, direct access
/// ❌ Cons: Hardcoded bindings, difficult to manage for multiple devices
/// </summary>
public class WaysToUseInputSystem_Polling : MonoBehaviour
{
    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Debug.Log("Any key was pressed");
        }

        if (Keyboard.current.spaceKey.IsPressed())
        {
            Debug.Log("Jump using polling");
        }

        //This null check
        if (Keyboard.current?.spaceKey.IsPressed() ?? false)
        {
            Debug.Log("Jump using polling");
        }

        if (Gamepad.current?.buttonSouth.wasPressedThisFrame ?? false)
        {
            Debug.Log("Gamepad button South (A on Xbox, X on PS) pressed!");
        }

        Gamepad gamepad = Gamepad.current;

        if(gamepad != null && gamepad.aButton.wasReleasedThisFrame)
        {

        }
    }
}