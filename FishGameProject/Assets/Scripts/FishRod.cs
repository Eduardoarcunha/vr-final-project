using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FishRod : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Transform midPoint;

    public Transform reelTransform;

    public Transform hookTransform;
    public Transform hookPosInicial;

    public Transform rodTransform;

    public Rigidbody hookRigidbody;

    private Vector3 previousHookPosition;
    private Vector3 currentHookPosition;

    private Vector3 previousVelocity;
    private Vector3 currentVelocity;
    private Vector3 velocityChange;

    private float previousRotation;
    private float throwForce; // The force applied to throw the hook
    public float hookSpeed;
    public bool isHookThrown = false; // Flag to track if the hook is thrown

    private InputDevice targetDevice;

    public InputDeviceCharacteristics controllerCharacteristics;

    public Hook hook;

    void Start()
    {
        previousRotation = reelTransform.localEulerAngles.y;
        previousHookPosition = hookRigidbody.position;
        previousVelocity = Vector3.zero;

        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer not assigned to FishRod script!");
            return;
        }

        TryInitialize();

    }

    void TryInitialize()
    {

        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }


    }
    void FixedUpdate()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        // Update the positions of the line renderer to simulate the fish line
        UpdateLinePositions();

        ThrowForceCalculation();

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue && !isHookThrown)
        {
            ThrowHook();
        }

        // Reel in the hook if it's thrown
        if (isHookThrown)
        {
            // Rotate the fishing rod based on device rotation
            RotateFishingRod();
            ReelInHook();
        }
    }

    void ThrowForceCalculation()
    {
        currentHookPosition = hookRigidbody.position;

        currentVelocity = (currentHookPosition - previousHookPosition) / Time.fixedDeltaTime;

        velocityChange = currentVelocity - previousVelocity;

        if (velocityChange.magnitude <= 20)
        {
            throwForce = 2;
        }

        else if (velocityChange.magnitude <= 50)
        {
            throwForce = 8;
        }

        else if (velocityChange.magnitude <= 75)
        {
            throwForce = 15;
        }

        previousHookPosition = currentHookPosition;
        previousVelocity = currentVelocity;

    }

    void UpdateLinePositions()
    {
        // Set the positions of the line renderer
        lineRenderer.positionCount = 3; // For simplicity, assume only two points: rod tip and hook

        lineRenderer.SetPosition(0, reelTransform.position); // Carretel position
        lineRenderer.SetPosition(1, midPoint.position);
        lineRenderer.SetPosition(2, hookTransform.position); // Hook position
    }


    void ThrowHook()
    {
        hookRigidbody.isKinematic = false; // Ensure hook's Rigidbody is not kinematic to enable physics
        hookRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse); // Apply forward force to throw the hook
        isHookThrown = true; // Set flag to indicate hook is thrown
    }

    void RotateFishingRod()
    {
        // Get the current rotation of the object
        float currentRotation = reelTransform.localEulerAngles.y;

        // Calculate the rotation direction (clockwise or counterclockwise) based on the change in angle
        float rotationDirection = currentRotation - previousRotation;

        // Check if the rotation is clockwise or counterclockwise
        if (rotationDirection > 0)
        {
            // hookTransform.Translate(Vector3.forward * hookSpeed * Time.fixedDeltaTime);
            hookTransform.Translate(Vector3.forward * 2 * Time.fixedDeltaTime);
            AudioManager.instance.PlaySound("FishRod");
        }
        else if (rotationDirection < 0)
        {
            hookTransform.position = Vector3.Lerp(hookTransform.position, rodTransform.position, hookSpeed * Time.fixedDeltaTime);
            AudioManager.instance.PlaySound("FishRod");
        }
        else
        {
            AudioManager.instance.StopSound("FishRod");
        }

        // Update the previous rotation for the next frame
        previousRotation = currentRotation;
    }

    void ReelInHook()
    {
        // Checagem para ver se está perto da vara
        float distanceToRodTip = Vector3.Distance(hookRigidbody.position, transform.position);
        if (distanceToRodTip < 1.3f)
        {
            hookRigidbody.isKinematic = true;
            hookTransform.position = hookPosInicial.position; // Volta a posição para a inicial
            isHookThrown = false; // Reseta a flag
            hook.onWater = false;
            AudioManager.instance.PlaySound("PullHook");
        }
    }
}
