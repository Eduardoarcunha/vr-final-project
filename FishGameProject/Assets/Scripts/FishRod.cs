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
    public Rigidbody hook_rb;

    private float previousRotation;
    public float throwForce = 10f; // The force applied to throw the hook
    public float moveSpeed = 1f;
    private bool isHookThrown = false; // Flag to track if the hook is thrown

    private InputDevice targetDevice;


    void Start()
    {
        previousRotation = BaseCarretel.localEulerAngles.y;
        
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer not assigned to FishRod script!");
            return;
        }

        // List<InputDevice> devices = new List<InputDevice>();
        // InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        // InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);    
        // print(devices+ " " + devices.Count);
        // foreach (var item in devices){
        //     Debug.Log(item.name);
        // }

        // List<InputDevice> devices = new List<InputDevice>();
        // InputDevices.GetDevices(devices);
        // print(devices+ " " + devices.Count);
        // print(devices[0].isValid);

        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        print(device.isValid);
        if(device.isValid)
        {
            targetDevice = device;
            Debug.Log("Hello");
        }
        else
        {
            Debug.Log("Não achei o XRNode.RightHand");
        }
    }
    void Update()
    {
        // Update the positions of the line renderer to simulate the fish line
        UpdateLinePositions();

        if (Input.GetKeyDown(KeyCode.Space) && !isHookThrown)
        // if (primaryButtonValue && !isHookThrown)
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
            hook_t.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (rotationDirection < -2f)
        {
            hook_t.Translate(Vector3.forward * -moveSpeed * Time.deltaTime);
        }
        else
        {
            // Debug.Log("No rotation" + rotationDirection);
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
            hook_rb.isKinematic = true;
            hook_t.position = hookPosInicial.position; // Volta a posição para a inicial
            isHookThrown = false; // Reseta a flag
        }
    }
}
