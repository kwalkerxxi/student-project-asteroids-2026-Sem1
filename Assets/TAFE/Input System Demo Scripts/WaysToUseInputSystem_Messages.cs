
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This shows using the Input System with 
/// Using SendMessage (Legacy Unity Approach)
/// You can invoke methods on the same GameObject dynamically.
/// ✔ Pros: Works without direct references
/// ❌ Cons: Performance overhead, no compile-time safety
/// 
/// Note:
///     SendMessage (Calls a Method on the Same GameObject)
///     ✔ Calls Jump() only on the scripts attached to this GameObject.
///     ❌ If no method named Jump exists, Unity logs a warning (unless SendMessageOptions.DontRequireReceiver is used).
/// whereas 
///     BroadcastMessage (Calls a Method on the GameObject and Its Children)
///     ✔ Reaches multiple scripts
///     ✔ Calls Jump() on this GameObject and all child GameObjects.
///     ❌ Can be inefficient for deep hierarchies because it propagates down the hierarchy.
///     ❌ Slower than direct calls, no error checking
/// </summary>
public class WaysToUseInputSystem_Messages : MonoBehaviour
{
    public void OnJump()
    {
        SendMessage("Jump");
        //or
        BroadcastMessage("Jump");
    }

    private void Jump()
    {
        Debug.Log("Jump (SendMessage or BroadcastMessage) triggered!");
    }

    [SerializeField] private Vector2 movementValue;
    [SerializeField] private float moveSpeed;
    bool modifierPressed;

    void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>(); // or a float type if just a button
    }

    void Update()
    {
        transform.Translate(new Vector3(movementValue.x, 0, movementValue.y) * moveSpeed * Time.deltaTime);
    }

    void OnModifier(InputValue value)
    {
        modifierPressed = value.isPressed;
    }
}

public class AnotherChildScript : MonoBehaviour
{
    private void Jump()
    {
        Debug.Log("Jump (BroadcastMessage) triggered!");
    }
}
