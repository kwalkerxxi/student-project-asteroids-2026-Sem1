using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A simple countdown timer that can invoke a UnityEvent when completed.
/// Supports optional looping and manual control through public methods.
/// </summary>
public class SimpleTimer : MonoBehaviour
{
    [Header("Timer Settings")]

    /// <summary>
    /// The length of the timer in seconds.
    /// </summary>
    [SerializeField] private float duration = 5f;

    /// <summary>
    /// If true, the timer will automatically start on Start().
    /// </summary>
    [SerializeField] private bool playOnStart = true;

    /// <summary>
    /// If true, the timer will restart automatically after completion.
    /// </summary>
    [SerializeField] private bool loop = false;

    [Header("Events")]

    /// <summary>
    /// Invoked when the timer reaches zero.
    /// </summary>
    public UnityEvent OnTimerComplete = new UnityEvent();

    private float timer;
    private bool isRunning;

    private void Start()
    {
        if (playOnStart)
        {
            StartTimer();
        }
    }

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            OnTimerComplete?.Invoke();

            if (loop)
            {
                timer = duration;
            }
            else
            {
                isRunning = false;
            }
        }
    }

    /// <summary>
    /// Starts the timer and resets it to the configured duration.
    /// </summary>
    public void StartTimer()
    {
        timer = duration;
        isRunning = true;
    }

    /// <summary>
    /// Stops the timer without resetting the remaining time.
    /// </summary>
    public void StopTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// Resets the timer back to the configured duration.
    /// Does not automatically start the timer.
    /// </summary>
    public void ResetTimer()
    {
        timer = duration;
    }
}