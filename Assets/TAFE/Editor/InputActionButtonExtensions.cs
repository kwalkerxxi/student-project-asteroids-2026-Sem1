using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// If wanting to use like old Input Class call
/// Obtained from: https://discussions.unity.com/t/solved-getbuttondown-getbuttonup-with-the-new-system/787563/8
/// </summary>
public static class InputActionButtonExtensions
{
    public static bool GetButton(this InputAction action) => action.ReadValue<float>() > 0;
    public static bool GetButtonDown(this InputAction action) => action.triggered && action.ReadValue<float>() > 0;
    public static bool GetButtonUp(this InputAction action) => action.triggered && action.ReadValue<float>() == 0;
}