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

    float timer = 0;
    [SerializeField] float restartWaitTime = 4;
    [SerializeField] TextMeshProUGUI gameStartAndEndTextboxCountDown;
    [SerializeField] QuickTweenScores quickTweenScores;
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

    private void CountDownToRestart()
    {
        timer -= Time.deltaTime;
        gameStartAndEndTextboxCountDown.text = timer.ToString("N0");
    }


    public void OnPlayerDied()
    {
        playersLeft--;
        Mathf.Clamp(playersLeft, 0, 10);
    }

    public void RegisterPlayer(PlayerCollisions player)
    {
        player.OnDied.AddListener(OnPlayerDied);
    }

    void Update()
    {
        if(isRestarting)
        {
            CountDownToRestart();
            return;
        }

        //playersLeft = PlayerInputManager.instance.playerCount;

        if(playersJoined > 0 && playersLeft <= 0)
        {
            if(gameStartAndEndTextbox != null)
            {
                //gameStartAndEndTextbox.text = $"Game Over! This game had {playersJoined} players in the session.";
                //gameStartAndEndTextbox.text += $"\nAll have died.";
                //gameStartAndEndTextbox.text += $"\nScene will reload and Inputs will be";
                //gameStartAndEndTextbox.text += $"\nre-enabled in 4 seconds.";
                gameStartAndEndTextbox.text = $"Game Over!";
                gameStartAndEndTextbox.text += $"\nAll players have died.";
                gameStartAndEndTextbox.text += $"\nScene will reload and Inputs will be";
                gameStartAndEndTextbox.text += $"\nre-enabled in 4 seconds.";

            }
            isRestarting = true;
            OnGameEnded?.Invoke();
            Invoke(nameof(ReloadScene), restartWaitTime);
            timer = restartWaitTime;
            quickTweenScores.GrowScores();
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
        playersLeft++;
    }


}
