using UnityEngine;

/// <summary>
/// Example Entity/Actor that caches all components
/// - other scripts would reference it
/// </summary>
public class Entity : MonoBehaviour
{
    //public Health Health { get; private set; }
    //public ScoreValue ScoreValue { get; private set; }

    void Awake()
    {
        //  Health = GetComponent<Health>();
        //  ScoreValue = GetComponent<ScoreValue>();
    }
}