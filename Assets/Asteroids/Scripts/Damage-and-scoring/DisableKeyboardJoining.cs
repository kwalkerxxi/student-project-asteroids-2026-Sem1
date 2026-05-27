using UnityEngine;
using UnityEngine.InputSystem;

public class DisableKeyboardJoining : MonoBehaviour
{
    public static bool isKeyboardJoingingAllowed = true;

    public void OnPlayerJoined(PlayerInput player)
    {
        if(!isKeyboardJoingingAllowed)
        {
            // If the joining device is keyboard or mouse, reject the join
            foreach(var device in player.devices)
            {
                if(device is Keyboard || device is Mouse)
                {
                    Destroy(player.gameObject);
                    return;
                }
            }
        }
    }
}
