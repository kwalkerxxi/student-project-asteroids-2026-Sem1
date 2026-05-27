using UnityEngine;

public class ExampleTickInsteadOfUpdate : MonoBehaviour, IUpdatable
{
    void OnEnable()
    {
        UpdateManager.Register(this);
    }

    void OnDisable()
    {
        UpdateManager.Unregister(this);
    }

    public void Tick()
    {
        transform.position += Vector3.forward * Time.deltaTime;
    }
}