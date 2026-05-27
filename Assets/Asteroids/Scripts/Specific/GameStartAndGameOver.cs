using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStartAndGameOver : MonoBehaviour
{
    int playersJoined = 0;
    int playersLeft = 0;
    [SerializeField] TextMeshProUGUI gameStartAndEndTextbox;

    [SerializeField] UnityEvent OnGameEnded;
    bool isRestarting = false;
    private void Start()
    {
        ReEnableAllDevices();
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
    private void ReloadScene()
    {
        isRestarting = false;
        DisableKeyboardJoining.isKeyboardJoingingAllowed = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if(isRestarting)
        {
            return;
        }

        playersLeft = PlayerInputManager.instance.playerCount;

        if(playersJoined > 0 && playersLeft <= 0)
        {
            if(gameStartAndEndTextbox != null)
            {
                gameStartAndEndTextbox.text = $"Game Over! This game had {playersJoined} players in the session.";
                gameStartAndEndTextbox.text += $"\nAll have died.";
                gameStartAndEndTextbox.text += $"\nScene will reload and Inputs will be";
                gameStartAndEndTextbox.text += $"\nre-enabled in 4 seconds.";

            }
            isRestarting = true;
            OnGameEnded?.Invoke();
            Invoke(nameof(ReloadScene), 4f);
        }
        else
        {
            if(gameStartAndEndTextbox != null)
            {
                gameStartAndEndTextbox.text = $"GAME STARTED!";
                gameStartAndEndTextbox.text += $"\nPress any button to join the game.";
                gameStartAndEndTextbox.text += $"\nCurrently, {playersJoined} players have joined in the session.";
                gameStartAndEndTextbox.text += $"\nCurrently, {playersJoined - playersLeft} have died :(";
            }
        }
    }

    public void PlayerAdded(PlayerInput input)
    {
        playersJoined++;
    }


}
