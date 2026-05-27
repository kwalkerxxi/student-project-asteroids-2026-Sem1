using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GridBrushBase;

/// <summary>
/// 
/// Author: Sebastian Rutherfoord
/// Description: Controls a set of jet engine flames.
/// 
/// </summary>
public class PlayerEngineFlame : MonoBehaviour
{
    // Class variables
    // Engines to use, it gets sorted by local X
    [SerializeField]
    private List<GameObject> flameFX = new List<GameObject>();
    [SerializeField]
    private float centralEngineDistance = 0.1f;
    

    // Setup variables
    private PlayerInput playerInput;
    private InputAction thrustInputAction;
    private InputAction rotationInputAction;


    private int lastLeftHandEngineIndex;
    private int firstRightHandEngineIndex;

    float thrustInput;
    Vector2 rotationInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flameFX.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        for (int i = 0; i < flameFX.Count; i++)
        {
            if (flameFX[i].transform.position.x < -centralEngineDistance)
            {
                lastLeftHandEngineIndex = i;
            }
            else if (flameFX[i].transform.position.x > centralEngineDistance)
            {
                firstRightHandEngineIndex = i;
            }
        }
    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

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
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Rotation Handling
        // Get rotation direction
        float rotationDirection = 0;
        if (isHoldingRotate)
        {
            rotationInput = rotationInputAction.ReadValue<Vector2>();
            float angle = Mathf.Atan2(rotationInput.y, rotationInput.x) * Mathf.Rad2Deg;

            string scheme = playerInput.currentControlScheme;

            if (scheme == "Keyboard&Mouse")
            {
                //Debug.Log("Using keyboard and mouse");
            rotationDirection = rotationInputAction.ReadValue<Vector2>().x; 
            }
            else if (scheme == "Gamepad")
            {
                //Debug.Log("Using gamepad");
            rotationDirection = (rotationInputAction.ReadValue<Vector2>().x + rotationInputAction.ReadValue<Vector2>().y); 
            }


        }

        // Activate engines based on rotation direction
        if (rotationDirection < -0.01f)
        {
            // Right on
            for (int i = 0; i < flameFX.Count; i++)
            {
                if (i >= firstRightHandEngineIndex)
                {
                    flameFX[i].GetComponent<ParticleSystem>().Play();
                }
                else if (i <= lastLeftHandEngineIndex)
                {
                    flameFX[i].GetComponent<ParticleSystem>().Stop();
                }
            }

        }
        else if (rotationDirection > 0.01f)
        {
            // Left on
            for (int i = 0; i < flameFX.Count; i++)
            {
                if (i >= firstRightHandEngineIndex)
                {
                    flameFX[i].GetComponent<ParticleSystem>().Stop();
                }
                else if (i <= lastLeftHandEngineIndex)
                {
                    flameFX[i].GetComponent<ParticleSystem>().Play();
                }
            }

        }
        else
        {
            // Turn the side thrusters off.
            for (int i = 0; i < flameFX.Count; i++)
            {
                if (i >= firstRightHandEngineIndex)
                {
                    flameFX[i].GetComponent<ParticleSystem>().Stop();
                }
                else if (i <= lastLeftHandEngineIndex)
                {
                    flameFX[i].GetComponent<ParticleSystem>().Stop();
                }
            }
        }

        // Forward acceleration handling
        if (thrustInput > 0.1f)
        {
            for (int i = 0; i < flameFX.Count; i++)
            {
                if (!(i <= lastLeftHandEngineIndex || i >= firstRightHandEngineIndex))
                {
                    flameFX[i].GetComponent<ParticleSystem>().Play();
                }
            }
        }
        else
        {
            for (int i = 0; i < flameFX.Count; i++)
            {
                if (!(i <= lastLeftHandEngineIndex || i >= firstRightHandEngineIndex))
                {
                    flameFX[i].GetComponent<ParticleSystem>().Stop();
                }
            }
        }
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

}
