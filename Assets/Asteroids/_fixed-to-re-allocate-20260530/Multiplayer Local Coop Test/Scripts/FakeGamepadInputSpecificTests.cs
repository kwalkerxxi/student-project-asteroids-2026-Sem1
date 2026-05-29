using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class FakeGamepadInputSpecificTests : MonoBehaviour
{
    private Gamepad virtualGamepad;

    Sequence pressSequence;
    void Start()
    {
        virtualGamepad = InputSystem.AddDevice<Gamepad>();
        var forExampleMappingPaths = virtualGamepad.allControls;

        //RunRandomPress(PressLeftShoulder);

        pressSequence = DOTween.Sequence();

        // 13 left presses
        for(int i = 0; i < 13; i++)
        {
            pressSequence.AppendCallback(PressLeftShoulder)
               .AppendInterval(1.5f);
        }

        // 15 right presses
        for(int i = 0; i < 15; i++)
        {
            pressSequence.AppendCallback(PressRightShoulder)
               .AppendInterval(1.5f);
        }

        // final button east
        // pressSequence.AppendCallback(PressButtonEast);
    }

    [ProPlayButton]
    void RunSequence()
    {
        pressSequence.Restart();
        pressSequence.Play();
    }

    void RunRandomPress(TweenCallback action)
    {
        float randomDuration = UnityEngine.Random.Range(1f, 2f);
        float randomTimer = 0f;
        Tween randomTimeTween = DOTween.To(() => randomTimer, x => randomTimer = x, randomDuration, randomDuration).OnComplete(action);
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
