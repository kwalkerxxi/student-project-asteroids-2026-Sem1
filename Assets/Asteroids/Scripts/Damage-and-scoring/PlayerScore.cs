using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScore : MonoBehaviour
{
    [field: SerializeField] public int Score { get; private set; }
    [field: SerializeField] public int PlayerIndex { get; private set; } = -1;

    public event Action<int, int> OnScoreChanged;
    // parameters: playerIndex, newScore

    void Start()
    {
        PlayerIndex = GetComponent<PlayerInput>().playerIndex;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(PlayerIndex, Score);
    }
}