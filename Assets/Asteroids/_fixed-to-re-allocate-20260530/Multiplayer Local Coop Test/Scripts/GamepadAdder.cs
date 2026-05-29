using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using DG.Tweening;

public class GamepadAdder : MonoBehaviour
{
    private List<Gamepad> addedGamepads = new List<Gamepad>();

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame) // Press T to add a Gamepad
        {
            AddGamepad();
        }
        if (Keyboard.current.yKey.wasPressedThisFrame) // Press T to add a Gamepad
        {
            for (int i = 0; i < 10; i++)
            {
            AddGamepad();
            }
        }
        if (Keyboard.current.rKey.wasPressedThisFrame) // Press R to remove all
        {
            RemoveAllGamepads();
        }

        if (addedGamepads.Count > 0 && Time.frameCount % 60 == 0)
        {
            foreach (var gamepad in addedGamepads)
            {
                SimulateStickMovement(gamepad, new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
            }
        }
    }

    void AddGamepad()
    {
        Gamepad newGamepad = InputSystem.AddDevice<Gamepad>();
        if (newGamepad != null)
        {
            addedGamepads.Add(newGamepad);
            Debug.Log($"Added Gamepad. Total: {addedGamepads.Count}");

            SimulateButtonPress(newGamepad, newGamepad.buttonNorth);
        }
        else
        {
            Debug.LogError("Failed to add Gamepad.");
        }
    }

    void RemoveAllGamepads()
    {
        foreach (var gamepad in addedGamepads)
        {
            InputSystem.RemoveDevice(gamepad);
        }
        addedGamepads.Clear();
        Debug.Log("Removed all added Gamepads.");
    }

    void OnDestroy()
    {
        RemoveAllGamepads();
    }

    void SimulateButtonPress(Gamepad gamepadToUse, ButtonControl button)
    {
        InputEventPtr eventPtr;
        using (StateEvent.From(gamepadToUse, out eventPtr))
        {
            button.WriteValueIntoEvent<float>(1, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
    }

    void SimulateStickMovement(Gamepad gamepadToUse, Vector2 stickMovement)
    {
        InputEventPtr eventPtr;
        using (StateEvent.From(gamepadToUse, out eventPtr))
        {
            gamepadToUse.leftStick.WriteValueIntoEvent(new Vector2(stickMovement.x, stickMovement.y), eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
    }

    void DoRandomLeftStickInput(Gamepad gamepadToControl)
    {
        SimulateStickMovement(gamepadToControl, new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
        RunRandomDOTweenAction(() => DoRandomLeftStickInput(gamepadToControl));

    }

    void RunRandomDOTweenAction(System.Action moo)
    {
        TweenCallback callback = new TweenCallback(moo);

        float randomDuration = UnityEngine.Random.Range(1f, 2f);
        float randomTimer = 0f;
        Tween randomTimeTween = DOTween.To(() => randomTimer, x => randomTimer = x, randomDuration, randomDuration).OnComplete(callback);
    }
}
