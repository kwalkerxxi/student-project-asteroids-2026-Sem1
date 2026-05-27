using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PauseScene : MonoBehaviour
{
    [SerializeField]
    List<MonoBehaviour> allScriptsToPause;
    [SerializeField] PlayerInputManager playerManager;
    [SerializeField] Transform pauseMenu;
    [SerializeField] Transform gameUI;
    [SerializeField] bool isPaused = false;

    
    [SerializeField] private InputAction PauseInput;


    [SerializeField] Button whatToSelectWhenPaused;
    private void OnEnable()
    {
        PauseInput.Enable();
        PauseInput.performed += OnPausePress;
    }

    private void OnDisable()
    {
        PauseInput.performed -= OnPausePress;
        PauseInput.Disable();
    }

    void OnPausePress(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {

            SwitchUI();
            ToggleScriptStates(false);

            Time.timeScale = 0;

            whatToSelectWhenPaused.Select();
        }
        else
        {
            UnPause();
        }

 
    }
     public void SwitchUI()
     {
            pauseMenu.gameObject.SetActive(isPaused);
            gameUI.gameObject.SetActive(!isPaused);
     }

    public void UnPause()
    {
        Time.timeScale = 1;
        isPaused = false;
        ToggleScriptStates(true);
        SwitchUI();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    void ToggleScriptStates(bool areScriptsRunning)
    {
        var allPlayerInputs = GameObject.FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID);

        playerManager.enabled = areScriptsRunning;
        allScriptsToPause.Clear();
        foreach (var item in allPlayerInputs)
        {
            allScriptsToPause.AddRange(item.GetComponentsInChildren<MonoBehaviour>());
        }

        
        foreach (var item in allScriptsToPause)
        {
            item.enabled = areScriptsRunning;
        }
    }
}
