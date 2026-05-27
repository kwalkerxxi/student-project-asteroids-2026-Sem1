using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// This script warps objects to a random spot on the screen
/// </summary>
public class WarpObject : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody cachedRigidbody;
    InputAction warpInputAction;



    bool isWarping = false;
    [SerializeField] float fixedY = 0;
    private static GameObject particleHolder;
    [SerializeField] GameObject ParticleSystemOnWarp;
    public UnityEvent OnWarping;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cachedRigidbody = GetComponent<Rigidbody>();
        warpInputAction = playerInput.actions["Jump"];
        if (particleHolder == null)
        {
            particleHolder = new GameObject("Particle Holder - Warps");
        }
    }
    private void OnEnable()
    {
        warpInputAction.performed += OnWarp;
    }
    private void OnDisable()
    {
        warpInputAction.performed -= OnWarp;
    }
    private void OnWarp(InputAction.CallbackContext context)
    {
        if (isWarping)
        {
            return;
        }
        if (ParticleSystemOnWarp != null)
        {
            GameObject warpParticleSystem = Instantiate(ParticleSystemOnWarp, particleHolder.transform);
            Vector3 wormholePosition = transform.position;
            wormholePosition.y -= 2;
            Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
            warpParticleSystem.transform.SetPositionAndRotation(wormholePosition, rotation);
            warpParticleSystem.AddComponent<AutoDestroyAfterParticlesEnd>();
        }
        isWarping = true;
        Invoke(nameof(WarpAway), 0.5f);
        Invoke(nameof(WarpToNewSpot), 1);
    }
    void WarpAway()
    {
        transform.position = ScreenPositionUtility.GetBehindCameraPosition(-2000f);
        OnWarping?.Invoke();
    }
    private void WarpToNewSpot()
    {
        isWarping = false;
        transform.position = ScreenPositionUtility.GetRandomOnScreenPosition(Camera.main, fixedY, 0.8f);
        cachedRigidbody.linearVelocity = Vector3.zero;
        cachedRigidbody.angularVelocity = Vector3.zero;
        if (ParticleSystemOnWarp != null)
        {
            GameObject warpParticleSystem = Instantiate(ParticleSystemOnWarp, particleHolder.transform);
            Vector3 wormholePosition = transform.position;
            wormholePosition.y -= 2;
            Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
            warpParticleSystem.transform.SetPositionAndRotation(wormholePosition, rotation);
            warpParticleSystem.AddComponent<AutoDestroyAfterParticlesEnd>();
        }
    }
}
