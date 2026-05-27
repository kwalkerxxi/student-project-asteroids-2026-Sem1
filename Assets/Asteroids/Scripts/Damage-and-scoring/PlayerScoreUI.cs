using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    public PlayerScore playerScore;
    public TextMeshProUGUI scoreText;

    public int visiblePlayerIndex = 0;

    public void SetPlayer(int playerIndex, PlayerScore scoreScript)
    {
        playerScore = scoreScript;
        playerScore.OnScoreChanged += UpdateScore;

        gameObject.SetActive(true);
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = $"P{playerIndex + 1}: 0";
    }

    void OnEnable()
    {
        if(playerScore)
        {
            playerScore.OnScoreChanged += UpdateScore;
        }
    }

    void OnDisable()
    {
        if(playerScore)
        {
            playerScore.OnScoreChanged -= UpdateScore;
        }
    }

    void UpdateScore(int playerIndex, int newScore)
    {
        scoreText.text = $"P{playerIndex + 1}: {newScore.ToString("N0")}";
    }
}