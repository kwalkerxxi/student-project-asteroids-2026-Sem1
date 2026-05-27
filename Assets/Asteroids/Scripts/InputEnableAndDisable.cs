using UnityEngine;
using UnityEngine.InputSystem;

public class InputEnableAndDisable : MonoBehaviour
{
    public void DisableKeyboardAndMouse()
    {
        if(Keyboard.current != null)
        {
            InputSystem.DisableDevice(Keyboard.current);
        }

        if(Mouse.current != null)
        {
            InputSystem.DisableDevice(Mouse.current);
        }

        Debug.Log("Keyboard and mouse disabled");
    }

    public void EnableKeyboardAndMouse()
    {
        if(Keyboard.current != null && !Keyboard.current.enabled)
        {
            InputSystem.EnableDevice(Keyboard.current);
        }

        if(Mouse.current != null && !Mouse.current.enabled)
        {
            InputSystem.EnableDevice(Mouse.current);
        }

        Debug.Log("Keyboard and mouse enabled");
    }
}