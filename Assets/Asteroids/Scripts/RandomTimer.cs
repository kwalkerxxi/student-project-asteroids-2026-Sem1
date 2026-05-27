using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This script runs a timer, using a random offset between a set minimum and maximum value
/// </summary>
public class RandomTimer : MonoBehaviour
{
    [System.Serializable]
    struct TimeRange
    {
        [Min(0)] public float min;
        [Min(0)] public float max;
    }

    public UnityEvent OnTimerInterval = new UnityEvent();

    [SerializeField] private TimeRange timeRange;
    float currentTime = 0;
    float timeToSpawnAt = 0;

    private void Start()
    {
        timeToSpawnAt = Random.Range(timeRange.min, timeRange.max);
    }

    void Update()
    {
        if(currentTime > timeRange.max)
        {
            timeToSpawnAt = Random.Range(timeRange.min, timeRange.max);
            currentTime -= timeToSpawnAt;
            OnTimerInterval?.Invoke();
            return;
        }

        currentTime += Time.deltaTime;
    }
}
