using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Destroy the EventSystem if there is already one in the scene
/// Needs the execution order to be negative as 
/// <see cref="UnityEngine.EventSystems.EventSystem"/> 
/// is set to -1000. 
/// The editor UI clamps values to ±32,000 when using the Script Execution Order window.
/// This uses <see cref="int.MinValue"/>
/// </summary>
[DefaultExecutionOrder(-2_147_483_647)]
public class RemoveDuplicateEventSystem : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
        //return;

        //if(GameObject.FindObjectsByType<InputSystemUIInputModule>(FindObjectsSortMode.None).Length > 1)
        //{
        //    Destroy(GetComponent<InputSystemUIInputModule>());
        //}

        //if(GameObject.FindObjectsByType<EventSystem>(FindObjectsSortMode.None).Length > 1)
        //{
        //    Destroy(GetComponent<EventSystem>());
        //}

        //Destroy(this);
    }
}
