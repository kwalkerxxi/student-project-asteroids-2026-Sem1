using DG.Tweening;
using NUnit.Framework.Internal;
using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.OnScreen;

public class FakeGamepadInput : MonoBehaviour
{
    private Gamepad virtualGamepad;

    void Start()
    {
        virtualGamepad = InputSystem.AddDevice<Gamepad>();

        var forExampleMappingPaths = virtualGamepad.allControls;

        //Invoke(nameof(PressButtonsToGetPlayer), Random.Range(2f, 5f));
        // Decided to use DOTween instead for later functionality
        float randomDuration = UnityEngine.Random.Range(2f, 5f);
        float randomTimer = 0f;
        Tween randomTimeTween = DOTween.To(() => randomTimer, x => randomTimer = x, randomDuration, randomDuration).OnComplete(PressButtonsToGetPlayer);


    }

    void PressButtonsToGetPlayer()
    {
        SimulateButtonPress(virtualGamepad, virtualGamepad.buttonNorth);
        RunRandomDOTweenAction(DoRandomLeftStickInput);
    }

    void DoRandomLeftStickInput()
    {
        SimulateStickMovement(virtualGamepad, new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
        RunRandomDOTweenAction(DoRandomLeftStickInput);

    }

    void RunRandomDOTweenAction(Action moo)
    {
        TweenCallback callback = new TweenCallback(moo);

        float randomDuration = UnityEngine.Random.Range(1f, 2f);
        float randomTimer = 0f;
        Tween randomTimeTween = DOTween.To(() => randomTimer, x => randomTimer = x, randomDuration, randomDuration).OnComplete(callback);
    }

    public static void SimulateButtonHold(Gamepad gamepadToUse, ButtonControl button)
    {
        InputEventPtr eventPtr;

        //Press
        using(StateEvent.From(gamepadToUse, out eventPtr))
        {
            button.WriteValueIntoEvent<float>(1, eventPtr);
            //gamepadToUse.buttonNorth.WriteValueIntoEvent<float>(1, eventPtr);
            // String Version also works
            //((ButtonControl)gamepadToUse["buttonSouth"]).WriteValueIntoEvent<float>(1, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
    }

    public static void SimulateButtonPress(Gamepad gamepadToUse, ButtonControl button)
    {
        InputEventPtr eventPtr;

        //Press
        using(StateEvent.From(gamepadToUse, out eventPtr))
        {
            button.WriteValueIntoEvent<float>(1, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }

        //Release
        using(StateEvent.From(gamepadToUse, out eventPtr))
        {
            button.WriteValueIntoEvent<float>(0f, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
    }

    public static void SimulateStickMovement(Gamepad gamepadToUse, Vector2 stickMovement)
    {
        InputEventPtr eventPtr;
        using(StateEvent.From(gamepadToUse, out eventPtr))
        {
            gamepadToUse.leftStick.WriteValueIntoEvent(new Vector2(stickMovement.x, stickMovement.y), eventPtr);
            // String Version also works
            //((StickControl)gamepadToUse["leftStick"]).WriteValueIntoEvent(new Vector2(.5f, .5f), eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the buttons are pressed for gamepad 1 and gamepad 2
        if(virtualGamepad.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Gamepad 1 South button pressed");
        }
    }

    private void OnDestroy()
    {
        InputSystem.RemoveDevice(virtualGamepad);
    }


}
