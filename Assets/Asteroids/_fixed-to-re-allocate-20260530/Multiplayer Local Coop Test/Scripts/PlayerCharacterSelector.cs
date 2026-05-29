using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterSelector : MonoBehaviour
{
    [field: SerializeField] public int PrefabIndex { get; set; }
    [field: SerializeField] public CustomPlayerInputManager customPlayerInputManager { get; set; }
    [field: SerializeField] public PlayerInput playerInput { get; set; }

    private int selectedSkin = 0;
    private bool isInTrigger = false;
    private float timeInTrigger = 0f;
    private float timeToActivateConfirmation = 3f;
    private bool isConfirmed = false;

    [SerializeField] GameObject[] CharactersToUse;

    private void OnEnable()
    {
        playerInput.actions["Previous"].performed += CharacterCycleLeft;
        playerInput.actions["Next"].performed += CharacterCycleRight;
        playerInput.actions["Confirm"].performed += ConfirmSelectionViaInput;

        selectedSkin = Random.Range(0, CharactersToUse.Length);
        DisplaySelectedSkin();
    }

    private void OnDisable()
    {
        playerInput.actions["Previous"].performed -= CharacterCycleLeft;
        playerInput.actions["Next"].performed -= CharacterCycleRight;
        playerInput.actions["Confirm"].performed -= ConfirmSelectionViaInput;
    }

    void CharacterCycleLeft(InputAction.CallbackContext context)
    {
        CycleCharacter(-1, customPlayerInputManager);
    }

    void CharacterCycleRight(InputAction.CallbackContext context)
    {
        CycleCharacter(1, customPlayerInputManager);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player enterd the confirmation zone set boolean to start timer
        if (other.CompareTag("ConfirmationZone"))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player left the confirmation zone, Reset the timer
        if (other.CompareTag("ConfirmationZone"))
        {
            isInTrigger = false;
            timeInTrigger = 0f;
        }
    }

    private void Update()
    {
        if (isInTrigger && !isConfirmed)
        {
            timeInTrigger += Time.deltaTime;
            if (timeInTrigger >= timeToActivateConfirmation)
            {
                ConfirmSelection();
            }
        }
    }

    public void ConfirmSelection()
    {
        if (!isConfirmed)
        {
            isConfirmed = true;
            Debug.Log($"Player {PrefabIndex} confirmed selection via trigger.");
            Destroy(this);
        }
    }

    public void ConfirmSelectionViaInput(InputAction.CallbackContext context)
    {
        ConfirmSelection();
    }

    public void CycleCharacter(float direction, CustomPlayerInputManager manager)
    {
        if (direction > 0)
        {
            //GOING UP - then loop to start of array
            selectedSkin = (++selectedSkin) % CharactersToUse.Length;
        }
        else if (direction < 0)
        {
            //GOING DOWN - then loop to end of array
            selectedSkin = (--selectedSkin + CharactersToUse.Length) % CharactersToUse.Length;
        }

        DisplaySelectedSkin();
    }

    private void DisplaySelectedSkin()
    {
        for (int i = 0; i < CharactersToUse.Length; i++)
        {
            CharactersToUse[i].SetActive(false);

            if (i == selectedSkin)
            {
                CharactersToUse[i].SetActive(true);
            }
        }
    }
}
