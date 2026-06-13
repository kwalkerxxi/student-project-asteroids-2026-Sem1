using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GenericInputRunsEvents : MonoBehaviour
{
    [SerializeField] InputAction inputAction;
    [SerializeField] UnityEvent unityEvent = new UnityEvent();
    [SerializeField] UnityEvent<bool> unityEventBoolToggle = new UnityEvent<bool>();

    bool toggleState = false;
    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.performed += RunEvent;
    }

    private void RunEvent(InputAction.CallbackContext context)
    {
        unityEvent?.Invoke();

        toggleState = !toggleState;
        unityEventBoolToggle?.Invoke(toggleState);
    }

    private void OnDisable()
    {
        inputAction.performed -= RunEvent;
        inputAction.Disable();
    }
}
