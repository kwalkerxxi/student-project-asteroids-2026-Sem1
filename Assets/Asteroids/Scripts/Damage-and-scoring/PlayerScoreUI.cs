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
        playerScore.OnScoreDisable += UpdateColor;

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

    void UpdateColor(int playerIndex)
    {

        scoreText.color = Color.red;
        // Face color (RGBA)
        scoreText.faceColor = new Color32(255, 0, 0, 128);   // Semi-transparent red

        // Outline color (RGBA)
        //scoreText.outlineColor = new Color32(0, 0, 0, 200);  // Mostly opaque black

        // Outline thickness (0–1 typically)
        //scoreText.outlineWidth = 0.2f;

        scoreText.text = $"X-" + scoreText.text;
        this.enabled = false;
    }

    void UpdateScore(int playerIndex, int newScore)
    {
        scoreText.text = $"P{playerIndex + 1}: {newScore.ToString("N0")}";
    }
}