using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MakePlaceholderPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    List<GameObject> fakePlayers = new List<GameObject>();

    private static GameObject fakePlayerHolder;

    private void Awake()
    {
        if(fakePlayerHolder == null)
        {
            fakePlayerHolder = new GameObject("Fake Player Holder");
        }
    }

    public void MakeTemporaryPlayer()
    {
        GameObject fakePlayer = Instantiate(playerPrefab, ScreenPositionUtility.GetRandomOnScreenPosition(Camera.main, transform.position.y), Quaternion.identity, fakePlayerHolder.transform);
        fakePlayers.Add(fakePlayer);
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if(keyboard != null && !keyboard.enabled)
        {
            //Debug.Log("Keyboard has been disabled");
            RemoveTemporaryPlayers();
        }

        var mouse = Mouse.current;
        if(mouse != null && !mouse.enabled)
        {
            //Debug.Log("Mouse has been disabled");
        }
    }

    public void RemoveTemporaryPlayers()
    {
        foreach(var player in fakePlayers)
        {
            Destroy(player);
        }
    }

    private void OnEnable()
    {
        Cheats.OnCreateFakePlayer += MakeTemporaryPlayer;
        Cheats.OnRemoveFakePlayers += RemoveTemporaryPlayers;
    }

    private void OnDisable()
    {
        Cheats.OnCreateFakePlayer -= MakeTemporaryPlayer;
        Cheats.OnRemoveFakePlayers -= RemoveTemporaryPlayers;
    }
}
