using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    public static Action OnToggleInfiniteBullets;
    public static Action<bool> OnToggleExtraGuns;
    public static Action OnClearScreenOfEnemies;
    public static Action OnResetScene;
    public static Action OnCreateFakePlayer;
    public static Action OnRemoveFakePlayers;

    bool isInGodMode = false;
    bool isUsingExtraGuns = false;
    public static Action<bool> OnToggleGodMode;

    [SerializeField] InputAction InfiniteBulletsInput;
    [SerializeField] InputAction ExtraGunsInput;
    [SerializeField] InputAction ClearScreenInput;
    [SerializeField] InputAction ResetSceneInput;
    [SerializeField] InputAction ToggleGodModeInput;
    [SerializeField] InputAction CreateFakePlayerInput;
    [SerializeField] InputAction RemoveFakePlayerInput;
    private void OnEnable()
    {
        InfiniteBulletsInput.Enable();
        ExtraGunsInput.Enable();
        ClearScreenInput.Enable();
        ResetSceneInput.Enable();
        ToggleGodModeInput.Enable();
        CreateFakePlayerInput.Enable();
        RemoveFakePlayerInput.Enable();

        InfiniteBulletsInput.performed += ToggleInfiniteBullets;
        ExtraGunsInput.performed += ToggleExtraGuns;
        ClearScreenInput.performed += ClearScreen;
        ResetSceneInput.performed += ResetScene;
        ToggleGodModeInput.performed += ToggleGodMode;
        CreateFakePlayerInput.performed += CreatFakePlayers;
        RemoveFakePlayerInput.performed += RemoveFakePlayers;
    }

    private void OnDisable()
    {
        InfiniteBulletsInput.performed -= ToggleInfiniteBullets;
        ExtraGunsInput.performed -= ToggleExtraGuns;
        ClearScreenInput.performed -= ClearScreen;
        ResetSceneInput.performed -= ResetScene;
        ToggleGodModeInput.performed -= ToggleGodMode;
        CreateFakePlayerInput.performed -= CreatFakePlayers;
        RemoveFakePlayerInput.performed -= RemoveFakePlayers;

        InfiniteBulletsInput.Disable();
        ExtraGunsInput.Disable();
        ClearScreenInput.Disable();
        ResetSceneInput.Disable();
        ToggleGodModeInput.Disable();
        CreateFakePlayerInput.Disable();
        RemoveFakePlayerInput.Enable();
    }

    private void ToggleInfiniteBullets(InputAction.CallbackContext context)
    {
        OnToggleInfiniteBullets?.Invoke();
    }

    private void ToggleExtraGuns(InputAction.CallbackContext context)
    {
        isUsingExtraGuns = !isUsingExtraGuns;
        OnToggleExtraGuns?.Invoke(isUsingExtraGuns);
    }

    private void ClearScreen(InputAction.CallbackContext context)
    {
        OnClearScreenOfEnemies?.Invoke();
    }

    private void CreatFakePlayers(InputAction.CallbackContext context)
    {
        OnCreateFakePlayer?.Invoke();
    }

    private void RemoveFakePlayers(InputAction.CallbackContext context)
    {
        OnRemoveFakePlayers?.Invoke();
    }

    private void ResetScene(InputAction.CallbackContext context)
    {

        //Needed this guard clause to stop multiple loads of scene
        if(Time.timeSinceLevelLoadAsDouble < 1f)
        {
            return;
        }

        OnResetScene?.Invoke();
        DisableKeyboardJoining.isKeyboardJoingingAllowed = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ToggleGodMode(InputAction.CallbackContext context)
    {
        isInGodMode = !isInGodMode;
        OnToggleGodMode?.Invoke(isInGodMode);
    }
}
