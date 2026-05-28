using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KonamiCode : MonoBehaviour
{
    public float WaitTime = 0.5f;

    public InputAction upAction;
    public InputAction downAction;
    public InputAction leftAction;
    public InputAction rightAction;
    public InputAction aAction;
    public InputAction bAction;

    public UnityEvent OnSuccess;

    
    private string[] expectedInputs = new string[]
    {
        "Up", "Up", "Down", "Down", "Left", "Right", "Left", "Right", "B", "A"
    };

    private float timer = 0f;
    private int index = 0;
    public bool success;

    private void OnEnable()
    {
        // Enable actions
        upAction.Enable();
        downAction.Enable();
        leftAction.Enable();
        rightAction.Enable();
        aAction.Enable();
        bAction.Enable();
    }

    private void OnDisable()
    {
        // Disable actions
        upAction.Disable();
        downAction.Disable();
        leftAction.Disable();
        rightAction.Disable();
        aAction.Disable();
        bAction.Disable();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
                index = 0;
            }
        }

        CheckInput(upAction, "Up");
        CheckInput(downAction, "Down");
        CheckInput(leftAction, "Left");
        CheckInput(rightAction, "Right");
        CheckInput(aAction, "A");
        CheckInput(bAction, "B");
    }

    private void CheckInput(InputAction action, string actionName)
    {
        if (action.triggered)
        {
            print(actionName);

            if (actionName == expectedInputs[index])
            {
                index++;

                if (index == expectedInputs.Length)
                {
                    success = true;
                    OnSuccess?.Invoke();
                    timer = 0f;
                    index = 0;
                }
                else
                {
                    timer = WaitTime;
                }
            }
            else
            {
                timer = 0;
                index = 0;
            }
        }
    }
}
