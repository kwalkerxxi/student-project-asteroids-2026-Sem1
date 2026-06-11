using System;
using Unity.Cinemachine;
using UnityEngine;
using HamishDelaforce;
namespace HamishDelaforce
{
    // <summary>
    // this script manages the Pick-ups when the player touches the pick-up by enabling
    // the power-up corresponding with the pick-up's tag for a limited amount of time
    // </Summary>
    public class PickUpManager : MonoBehaviour
    {
        private float Clock;
        // PowerUpTime determines the duration of the Power up
        [SerializeField] float PowerUpTime = 5f;
        bool timerIsActive = false;
        public static Action<bool> OnToggleExtraGuns;
        public static Action<bool> OnToggleGodMode;
        bool isUsingExtraGuns = false;
        bool isUsingInfiniteAmmo = false;
        bool isInGodMode = false;
        [SerializeField]
        [TagField] string tagForMoreGuns;
        [SerializeField]
        [TagField] string tagForInfiniteAmmo;
        [SerializeField]
        [TagField] string tagForShield;
        // when Clock equals PowerUpTime toggle the active Power up
        private void Update()
        {
            if (Clock >= PowerUpTime)
            {
                timerIsActive = false;
                Clock = 0f;
                if (isUsingExtraGuns == true)
                {
                    ToggleExtraGuns();
                }
                if (isUsingInfiniteAmmo == true)
                {
                    Cheats.OnToggleInfiniteBullets?.Invoke();
                    isUsingInfiniteAmmo = false;
                }
                if (isInGodMode == true)
                {
                    ToggleShield();                    
                }
            }
            // if timerIsActive is true start counting
            else if (timerIsActive)
            {
                Clock += Time.deltaTime;
            }
        }
        // when entering trigger with the tag formoreguns toggle ExtraGuns and if tagforInfiniteAmmo toggle infinitebullets
        // then starts timer and destroys the trigger
        private void OnTriggerEnter(Collider whatWasHit)
        {
            if (whatWasHit.gameObject.CompareTag(tagForMoreGuns))
            {
                ToggleExtraGuns();
                timerIsActive = true;
                Destroy(whatWasHit.gameObject);
            }
            if (whatWasHit.gameObject.CompareTag(tagForInfiniteAmmo))
            {
                Cheats.OnToggleInfiniteBullets?.Invoke();
                isUsingInfiniteAmmo = true;
                timerIsActive = true;
                Destroy(whatWasHit.gameObject);
            }
            if (whatWasHit.gameObject.CompareTag(tagForShield))
            {
                ToggleShield();
                timerIsActive = true;
                Destroy(whatWasHit.gameObject);
            }
        }
        // toggles the extra guns
        private void ToggleExtraGuns()
        {
            isUsingExtraGuns = !isUsingExtraGuns;
            OnToggleExtraGuns?.Invoke(isUsingExtraGuns);
        }

        private void ToggleShield()
        {
            isInGodMode = !isInGodMode;
            OnToggleGodMode?.Invoke(isInGodMode);
        }

    }
}