using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

public class SimpleInputMove : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction thrustInputAction;
    InputAction rotationInputAction;

    [SerializeField] float speed = 2f;
    [SerializeField] float rotationSpeed = 0.2f;
    [SerializeField] float rotationDampingFactor = 0.95f;
    [SerializeField] float speedDampingFactor = 0.99f;
    [SerializeField] float minimumThreshold = 0.1f;
    [SerializeField] private float maximumThreshold = 1f;

    [SerializeField] float maximumRotationThreshold = 1f;
    Rigidbody cachedRigidbody;
    float thrustInput;
    Vector2 rotationInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cachedRigidbody = GetComponent<Rigidbody>();

        thrustInputAction = playerInput.actions["Thrust"];
        rotationInputAction = playerInput.actions["Rotate"];
    }

    private void OnEnable()
    {
        thrustInputAction.performed += OnThrust;
        thrustInputAction.canceled += OnThrust;


        rotationInputAction.performed += OnRotateStarted;
        rotationInputAction.canceled += OnRotateCanceled;
    }

    private void OnDisable()
    {
        thrustInputAction.performed -= OnThrust;
        thrustInputAction.canceled -= OnThrust;

        rotationInputAction.performed -= OnRotateStarted;
        rotationInputAction.canceled -= OnRotateCanceled;
    }

    private void OnThrust(InputAction.CallbackContext cc)
    {
        thrustInput = cc.ReadValue<float>();
    }

    public void OnRotate(InputAction.CallbackContext cc)
    {
        rotationInput = cc.ReadValue<Vector2>();
    }

    bool isHoldingRotate;

    private void OnRotateStarted(InputAction.CallbackContext ctx)
    {
        isHoldingRotate = true;
    }

    private void OnRotateCanceled(InputAction.CallbackContext ctx)
    {
        isHoldingRotate = false;
    }

    public void OnLook(InputAction.CallbackContext cc)
    {

    }

    public void OnFire(InputAction.CallbackContext cc)
    {

    }

    void FixedUpdate()
    {
        float rotationDirection;
        if(isHoldingRotate)
        {
            rotationInput = rotationInputAction.ReadValue<Vector2>();
            //rotationDirection = Mathf.Sign(rotationInput.x); // -1 (left), 0, or 1 (right)

            ////Normalize to Force Full Strength
            //float direction = Mathf.Sign(rotationInput.x);
            //float strength = Mathf.Abs(rotationInput.x) > 0.1f ? 1f : 0f;
            //rotationDirection = direction * strength;


            ////Binary Direction
            ////Full X in left/right hemisphere
            ////Result:
            ////      Up + slight right → full speed right
            ////      Up + slight left → full speed left
            ////      Near center → no rotation

            //rotationDirection = 0f;
            //if(rotationInput.x > 0.1f)
            //{
            //    rotationDirection = 1f;
            //}
            //else if(rotationInput.x < -0.1f)
            //{
            //    rotationDirection = -1f;
            //}

            //Hemisphere-Based (Angle Method)
            //if(rotationInput.magnitude > 0.1f)
            //{
            float angle = Mathf.Atan2(rotationInput.y, rotationInput.x) * Mathf.Rad2Deg;
            rotationDirection = (angle > -90f && angle < 90f) ? 1f : -1f;
            //}


            // Optional deadzone to avoid jitter - this can also be set in Input System
            if(Mathf.Abs(rotationInput.x) < 0.1f)
            {
                rotationDirection = 0;
            }
        }
        else
        {
            rotationDirection = 0;
        }

        if(!(cachedRigidbody.linearVelocity.magnitude > maximumThreshold))
        {
            //cachedRigidbody.linearVelocity = Vector3.zero;
            cachedRigidbody.AddRelativeForce(Vector3.forward * thrustInput * speed, ForceMode.VelocityChange);
        }


        if(!(cachedRigidbody.angularVelocity.magnitude > maximumRotationThreshold))
        {

            cachedRigidbody.AddRelativeTorque(Vector3.up * rotationDirection * rotationSpeed, ForceMode.VelocityChange);
        }

        // Apply damping to the angular velocity
        cachedRigidbody.angularVelocity *= rotationDampingFactor;

        // Optional: Stop angular velocity if it's very low
        if(cachedRigidbody.angularVelocity.magnitude < minimumThreshold)
        {
            cachedRigidbody.angularVelocity = Vector3.zero;
        }




        // Optional: Stop angular velocity if it's very low
        if(cachedRigidbody.linearVelocity.magnitude < minimumThreshold)
        {
            cachedRigidbody.linearVelocity = Vector3.zero;
        }




    }
}
