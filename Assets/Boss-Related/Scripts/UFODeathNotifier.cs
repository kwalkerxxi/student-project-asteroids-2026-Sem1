using UnityEngine;

public class UFODeathNotifier : MonoBehaviour
{
    public UFOSpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.UFODestroyed();
        }
    }
}
