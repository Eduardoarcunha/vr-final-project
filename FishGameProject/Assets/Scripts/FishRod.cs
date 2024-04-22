using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FishRod : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform MidPosition;
    public Transform CarretelPosition;
    public Transform BaseCarretel;
    public Transform hook_t;
    public Transform hookPosInicial;
    public Transform vara_t;
    public Rigidbody hook_rb;
    private Vector3 previousPositionhook;
    private Vector3 currentPositionhook;
    private Vector3 previousVelocityhook;
    private Vector3 currentVelocityhook;
    private Vector3 velocityChange;
    private Vector3 acceleration;

    private float previousRotation;
    private float throwForce; // The force applied to throw the hook
    public float hookSpeed;
    public bool isHookThrown = false; // Flag to track if the hook is thrown

    private InputDevice targetDevice;

    public InputDeviceCharacteristics controllerCharacteristics;

    public AudioSource src;
    public AudioClip RodandoVara;
    public AudioClip HookVoltou;

    public GameObject ocean;
    
    private TriggerHook TH;


    void Start()
    {
        TH = ocean.GetComponent<TriggerHook>();

        previousRotation = BaseCarretel.localEulerAngles.y;
        previousPositionhook = hook_rb.position;
        previousVelocityhook = Vector3.zero;

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
        currentPositionhook = hook_rb.position;

        currentVelocityhook = (currentPositionhook - previousPositionhook) / Time.deltaTime;

        velocityChange = currentVelocityhook - previousVelocityhook;

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

        previousPositionhook = currentPositionhook;
        previousVelocityhook = currentVelocityhook;

    }

    void UpdateLinePositions()
    {
        // Set the positions of the line renderer
        lineRenderer.positionCount = 3; // For simplicity, assume only two points: rod tip and hook

        lineRenderer.SetPosition(0, CarretelPosition.position); // Carretel position
        lineRenderer.SetPosition(1, MidPosition.position);
        lineRenderer.SetPosition(2, hook_t.position); // Hook position
    }


    void ThrowHook()
    {
        hook_rb.isKinematic = false; // Ensure hook's Rigidbody is not kinematic to enable physics
        hook_rb.AddForce(transform.forward * throwForce, ForceMode.Impulse); // Apply forward force to throw the hook
        isHookThrown = true; // Set flag to indicate hook is thrown
    }

    void RotateFishingRod()
    {
        // Get the current rotation of the object
        float currentRotation = BaseCarretel.localEulerAngles.y;

        // Calculate the rotation direction (clockwise or counterclockwise) based on the change in angle
        float rotationDirection = currentRotation - previousRotation;

        // Check if the rotation is clockwise or counterclockwise
        if (rotationDirection > 2f)
        {
            // hook_t.Translate(Vector3.forward * hookSpeed * Time.deltaTime);
            hook_t.Translate(Vector3.forward * 2 * Time.deltaTime);
            AudioManager.instance.PlaySound("FishRod");
        }
        else if (rotationDirection < -2f)
        {
            hook_t.position = Vector3.Lerp(hook_t.position, vara_t.position, hookSpeed * Time.deltaTime);
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
        float distanceToRodTip = Vector3.Distance(hook_rb.position, transform.position);
        if (distanceToRodTip < 1.3f)
        {
            // hook_rb.isKinematic = true;
            hook_t.position = hookPosInicial.position; // Volta a posição para a inicial
            isHookThrown = false; // Reseta a flag
            TH.isHookWater = false;
            AudioManager.instance.PlaySound("PullHook");
        }
    }
}
