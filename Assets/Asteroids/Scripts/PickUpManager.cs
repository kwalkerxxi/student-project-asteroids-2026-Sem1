using System;
using System.Threading;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{

    private float Clock; // counts the second
    [SerializeField] float PowerUpTime = 5f; // the time the Clock resets
    bool Timer = false; // bool determines when Clock starts

    // toggles the extra guns
    public static Action<bool> OnToggleExtraGuns;
    bool isUsingExtraGuns = false;

    [SerializeField]
    [TagField] string tagForMoreGuns;

    private void Update() // updates every frame
    {

        if(Clock >= PowerUpTime) // check if Clock equals PowerUpTime
        {
            Timer = false; // stop the Timer
            Clock = 0f; // reset Clock to 0

            ToggleExtraGuns(); // disable Extra Guns
        }

        else if(Timer == true) // check if Timer is true
        {
            Clock += Time.deltaTime; // make clock start counting
        }
    }
    private void OnTriggerEnter(Collider whatWasHit) // when player collides with object
    {
        if (whatWasHit.gameObject.CompareTag(tagForMoreGuns))// check if object tag matches
        {
            ToggleExtraGuns(); // activate power up

            Timer = true; // start Timer
            Destroy(whatWasHit.gameObject); // destroy object
        }
    }

    private void ToggleExtraGuns() // this toggles the extra guns
    {
            isUsingExtraGuns = !isUsingExtraGuns;
            OnToggleExtraGuns?.Invoke(isUsingExtraGuns);
    }

}
