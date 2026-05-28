using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoiningBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerScoreUI[] scoreDisplays = new PlayerScoreUI[4];
    [SerializeField] private Transform[] playerTransforms = new Transform[4];

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

    public static Transform RandomPlayerToTarget;


    public Transform SetRandomActivePlayer()
    {
        List<Transform> activePlayers = new List<Transform>();

        foreach(Transform transformFound in playerTransforms)
        {
            if(transformFound == null)
            {
                continue;
            }

            activePlayers.Add(transformFound);
        }

        if(activePlayers.Count <= 0)
        {
            return null;
        }

        RandomPlayerToTarget = activePlayers[Random.Range(0, activePlayers.Count)];
        return RandomPlayerToTarget;
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

        if(index < playerTransforms.Length)
        {
            playerTransforms[index] = input.transform;
            SetRandomActivePlayer();

            // input.gameObject.GetComponent<PlayerCollisions>().OnDied.AddListener(() => SetRandomActivePlayer());
        }
    }
}
