using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoiningBehaviour : MonoBehaviour
{
    public PlayerScoreUI[] scoreDisplays = new PlayerScoreUI[4];

    private void Start()
    {
        for(int i = 0; i < scoreDisplays.Length; i++)
        {
            if(scoreDisplays[i] == null || scoreDisplays[i].gameObject == null)
            {
                continue;
            }

            scoreDisplays[i].gameObject.SetActive(false);
        }
    }
    public void OnPlayerJoined(PlayerInput input)
    {
        PlayerScore scoreScript = input.GetComponent<PlayerScore>();
        int index = input.playerIndex;

        //print(index);

        if(index < scoreDisplays.Length)
        {
            if(scoreDisplays[index] == null || scoreDisplays[index].gameObject == null)
            {
                return;
            }

            scoreDisplays[index].SetPlayer(input.playerIndex, scoreScript);
        }
    }
}
