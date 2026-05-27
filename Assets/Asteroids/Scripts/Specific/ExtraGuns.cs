using System;
using UnityEngine;

public class ExtraGuns : MonoBehaviour
{
    [SerializeField] private Transform[] extraGuns;

    private void Start()
    {
        foreach(Transform gun in extraGuns)
        {
            gun.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        Cheats.OnToggleExtraGuns += ToggleGuns;
        PickUpManager.OnToggleExtraGuns += ToggleGuns;
    }

    private void OnDisable()
    {
        Cheats.OnToggleExtraGuns -= ToggleGuns;
        PickUpManager.OnToggleExtraGuns -= ToggleGuns;
    }

    private void ToggleGuns(bool showGuns)
    {

        foreach(Transform gun in extraGuns)
        {
            gun.gameObject.SetActive(showGuns);
        }
    }
}
