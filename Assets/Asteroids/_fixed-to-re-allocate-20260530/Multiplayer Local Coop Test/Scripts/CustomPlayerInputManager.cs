using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerInputManager))]
public class CustomPlayerInputManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    [SerializeField] bool overridePlayerInputManagerMaxPlayerCount = false;
    [SerializeField] int customMaxPlayerCount = 120;

    private List<int> selectedPrefabIndices = new List<int>(); // Tracks selected prefabs
    private int currentPlayerIndex = -1;

    // Reference to the virtual gamepad
    private Gamepad virtualGamepad;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerLeft += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;

        virtualGamepad = new Gamepad();
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerLeft -= OnPlayerJoined;
        playerInputManager.onPlayerLeft -= OnPlayerLeft;
    }

    void OnDestroy()
    {
        // Nullify the virtual gamepad reference to let it be garbage collected
        virtualGamepad = null;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        currentPlayerIndex++;
       
        // Spawn in Single line technique
        //playerInput.transform.position += Vector3.right * (currentPlayerIndex * 2);

        // Spawn around in circle technique
        int totalItems = playerInputManager.maxPlayerCount;
        if(overridePlayerInputManagerMaxPlayerCount)
        {
            totalItems = customMaxPlayerCount;
        }
        // Assuming the radius of the circle is a predefined value
        float radius = 3f;  // Adjust this to fit the desired circle size
        float angleIncrement = 360f / totalItems;  // Determines the angle increment per item
        // Calculate the angle for the current index
        float angle = angleIncrement * currentPlayerIndex;
        // Convert the angle to radians
        float angleInRadians = Mathf.Deg2Rad * angle;
        // Calculate the x and y (or x and z for 3D) positions based on the angle
        float x = Mathf.Cos(angleInRadians) * radius;
        float z = Mathf.Sin(angleInRadians) * radius;
        // Set the new position for the player
        playerInput.transform.position = new Vector3(x, 0f, z); 

        SpawnPlayerWithPrefab(playerInput, currentPlayerIndex);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        // Free up the prefab when a player leaves
        int prefabIndex = playerInput.GetComponent<PlayerCharacterSelector>().PrefabIndex;
        selectedPrefabIndices.Remove(prefabIndex);
    }

    private void SpawnPlayerWithPrefab(PlayerInput playerInput, int prefabIndex)
    {
        selectedPrefabIndices.Add(prefabIndex);

        //Transform spawnPosition = playerInput.transform;
        PlayerCharacterSelector playerCharacter = playerInput.GetComponent<PlayerCharacterSelector>();
        playerCharacter.PrefabIndex = prefabIndex;
        playerCharacter.customPlayerInputManager = this;
        playerCharacter.playerInput = playerInput;
    }
}

/* SPAWNING IN EVEN INCREMENTS IN CIRCLE AROUND CENTER

// Assuming the radius of the circle is a predefined value
float radius = 5f;  // Adjust this to fit the desired circle size
float angleIncrement = 360f / totalItems;  // Determines the angle increment per item

// Loop through each item and place it in a circular pattern
for (int currentPlayerIndex = 0; currentPlayerIndex < totalItems; currentPlayerIndex++)
{
    // Calculate the angle for the current index
    float angle = angleIncrement * currentPlayerIndex;

    // Convert the angle to radians
    float angleInRadians = Mathf.Deg2Rad * angle;

    // Calculate the x and y (or x and z for 3D) positions based on the angle
    float x = Mathf.Cos(angleInRadians) * radius;
    float z = Mathf.Sin(angleInRadians) * radius;

    // Set the new position for the player
    playerInput.transform.position = new Vector3(x, 0f, z);  // y = 0 for 2D circle, modify for 3D if needed
}

*/