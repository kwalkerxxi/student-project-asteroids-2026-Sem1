using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    static List<IUpdatable> tickObjects = new List<IUpdatable>();

    public static void Register(IUpdatable obj)
    {
        tickObjects.Add(obj);
    }

    public static void Unregister(IUpdatable obj)
    {
        tickObjects.Remove(obj);
    }

    void Update()
    {
        for(int i = 0; i < tickObjects.Count; i++)
        {
            tickObjects[i].Tick();
        }
    }
}