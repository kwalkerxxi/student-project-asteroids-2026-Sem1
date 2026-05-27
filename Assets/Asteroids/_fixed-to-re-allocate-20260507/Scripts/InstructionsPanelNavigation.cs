using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsPanelNavigation : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] navigationCanvasGroups;
    [SerializeField] private string sceneToGoBackTo = "MainMenu";

    private int selectedIndex = 0;

    [Header("On Screen Buttons - for Touch")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button backButton;

    [Header("Control Mapped Buttons")]
    [SerializeField] InputAction leftInputButton;
    [SerializeField] InputAction rightInputButton;
    [SerializeField] InputAction backInputButton;


    [Header("Keyboard Images")]
    [SerializeField] private Image leftKeyboardImage;
    [SerializeField] private Image rightKeyboardImage;
    [SerializeField] private Image backKeyboardImage;

    [Header("Gamepad Images")]
    [SerializeField] private Image leftGamepadImage;
    [SerializeField] private Image rightGamepadImage;
    [SerializeField] private Image backGamepadImage;

    private bool usingGamepad = false;
    private Vector3 originalButtonScale = Vector3.one;

    private void Awake()
    {
        // Keyboard
        leftKeyboardImage.enabled = false;
        rightKeyboardImage.enabled = false;
        backKeyboardImage.enabled = false;

        // Gamepad
        leftGamepadImage.enabled = false;
        rightGamepadImage.enabled = false;
        backGamepadImage.enabled = false;

        FadeInChosenCanvasGroup();
    }
    /// <summary>
    /// These are currently disabled in the <see cref="PlayerCollisions.Die"/>
    /// This allows the devices to be used again for the next round
    /// </summary>
    private void ReEnableAllDevices()
    {
        foreach(var device in InputSystem.devices)
        {
            InputSystem.EnableDevice(device);
        }
    }

    private void OnEnable()
    {
        ReEnableAllDevices();

        leftButton?.onClick.AddListener(PreviousCanvasGroup);
        rightButton?.onClick.AddListener(NextCanvasGroup);
        backButton?.onClick.AddListener(GoBack);

        leftInputButton.Enable();
        rightInputButton.Enable();
        backInputButton.Enable();
        leftInputButton.performed += PreviousCanvasGroup;
        rightInputButton.performed += NextCanvasGroup;
        backInputButton.performed += GoBack;


        //leftInputButton.performed += OnInputUsed;
        //rightInputButton.performed += OnInputUsed;
        //backInputButton.performed += OnInputUsed;

        InputSystem.onEvent += OnAnyInput;
    }


    private void OnDisable()
    {
        InputSystem.onEvent -= OnAnyInput;

        leftButton?.onClick.RemoveListener(PreviousCanvasGroup);
        rightButton?.onClick.RemoveListener(NextCanvasGroup);
        backButton?.onClick.RemoveListener(GoBack);

        leftInputButton.performed -= PreviousCanvasGroup;
        rightInputButton.performed -= NextCanvasGroup;
        backInputButton.performed -= GoBack;

        //leftInputButton.performed -= OnInputUsed;
        //rightInputButton.performed -= OnInputUsed;
        //backInputButton.performed -= OnInputUsed;

        leftInputButton.Disable();
        rightInputButton.Disable();
        backInputButton.Disable();
    }

    private void OnAnyInput(InputEventPtr eventPtr, InputDevice device)
    {
        bool isGamepad = device is Gamepad;

        if(isGamepad != usingGamepad)
        {
            usingGamepad = isGamepad;
            //Debug.Log("Switched to: " + (isGamepad ? "Gamepad" : "Keyboard/Mouse"));
        }

        UpdateIcons();
    }

    //private void OnInputUsed(InputAction.CallbackContext context)
    //{
    //    InputDevice device = context.control.device;

    //    bool isGamepad = device is Gamepad;

    //    if(isGamepad != usingGamepad)
    //    {
    //        usingGamepad = isGamepad;
    //    }

    //    UpdateIcons();
    //}

    private void UpdateIcons()
    {
        // Keyboard
        leftKeyboardImage.enabled = !usingGamepad;
        rightKeyboardImage.enabled = !usingGamepad;
        backKeyboardImage.enabled = !usingGamepad;

        // Gamepad
        leftGamepadImage.enabled = usingGamepad;
        rightGamepadImage.enabled = usingGamepad;
        backGamepadImage.enabled = usingGamepad;
    }

    public void GoBack()
    {
        SceneManager.LoadSceneAsync(sceneToGoBackTo, LoadSceneMode.Single);
        AnimateButtonPress(backButton);
    }

    private void GoBack(InputAction.CallbackContext context)
    {
        GoBack();
    }

    void AnimateButtonPress(Button buttonToAnimate)
    {
        buttonToAnimate.transform.DOKill();
        buttonToAnimate.transform.localScale = originalButtonScale;
        buttonToAnimate.transform.DOPunchScale(Vector3.one * -0.5f, 0.2f);
    }

    public void NextCanvasGroup()
    {
        //selectedIndex++;
        selectedIndex = (++selectedIndex) % navigationCanvasGroups.Length;
        FadeInChosenCanvasGroup();

        AnimateButtonPress(rightButton);
    }

    private void NextCanvasGroup(InputAction.CallbackContext context)
    {
        NextCanvasGroup();
    }

    public void PreviousCanvasGroup()
    {
        //selectedIndex--;
        selectedIndex = (--selectedIndex + navigationCanvasGroups.Length) % navigationCanvasGroups.Length;
        FadeInChosenCanvasGroup();

        AnimateButtonPress(leftButton);
    }
    private void PreviousCanvasGroup(InputAction.CallbackContext context)
    {
        PreviousCanvasGroup();
    }



    void FadeInChosenCanvasGroup()
    {
        navigationCanvasGroups[selectedIndex].DOFade(1, 0.2f);

        for(int i = 0; i < navigationCanvasGroups.Length; i++)
        {
            if(i == selectedIndex)
            {
                continue;
            }
            navigationCanvasGroups[i].DOFade(0, 0.2f);

        }
    }
}
