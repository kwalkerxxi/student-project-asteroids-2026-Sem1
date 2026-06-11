using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
public class FakeGamepadInputTestPlayer : MonoBehaviour
{
    private Gamepad virtualGamepad;

    void Start()
    {
        virtualGamepad = InputSystem.AddDevice<Gamepad>();
        var forExampleMappingPaths = virtualGamepad.allControls;

        InvokeRepeating(nameof(PressButtonSouth), 0.2f, 0.2f);
        InvokeRepeating(nameof(MoveLeftStick), 0.3f, 0.3f);
    }

    private void Update()
    {
        HoldRightTrigger();
    }

    void RunRandomPress(TweenCallback action)
    {
        float randomDuration = UnityEngine.Random.Range(1f, 2f);
        float randomTimer = 0f;
        Tween randomTimeTween = DOTween.To(() => randomTimer, x => randomTimer = x, randomDuration, randomDuration).OnComplete(action);
    }


    void MoveLeftStick()
    {
        //FakeGamepadInput.SimulateButtonPress(virtualGamepad, virtualGamepad.rightTrigger);
        FakeGamepadInput.SimulateStickMovement(virtualGamepad, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
    }

    void HoldRightTrigger()
    {
        //FakeGamepadInput.SimulateButtonPress(virtualGamepad, virtualGamepad.rightTrigger);
        FakeGamepadInput.SimulateButtonHold(virtualGamepad, virtualGamepad.rightTrigger);
    }

    void PressButtonSouth()
    {
        FakeGamepadInput.SimulateButtonPress(virtualGamepad, virtualGamepad.buttonSouth);
    }







    void PressLeftShoulder()
    {
        FakeGamepadInput.SimulateButtonPress(virtualGamepad, virtualGamepad.leftShoulder);
    }

    void PressRightShoulder()
    {
        FakeGamepadInput.SimulateButtonPress(virtualGamepad, virtualGamepad.rightShoulder);
    }

    [ProButton]
    void PressButtonEast()
    {
        FakeGamepadInput.SimulateButtonPress(virtualGamepad, virtualGamepad.buttonEast);
    }
}
