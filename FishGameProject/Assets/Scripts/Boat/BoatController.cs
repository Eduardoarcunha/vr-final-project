using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class BoatController : MonoBehaviour
{
    [Header("Boat Movement")]
    public float accelerationSpeed;
    public float decelerationSpeed;
    public float turnSpeed;
    public float backwardSpeedLimit;
    private float currentSpeed = 0.0f;

    [Header("Boat Control")]
    public XRSlider engineSlider;
    public XRKnob steeringKnob;

    public GameObject boatFront;

    public float verticalInput;
    public float horizontalInput;

    [Header("Discrete Steering")]
    public int steeringSteps = 5;  // Number of discrete steps for steering

    void Update()
    {
        verticalInput = engineSlider.value * 2 - 1;
        horizontalInput = QuantizeInput(steeringKnob.value * 2 - 1, steeringSteps);

        // Movement logic
        HandleMovement(verticalInput);

        // Turning logic
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
    }

    private float QuantizeInput(float input, int steps)
    {
        float stepSize = 2f / (steps - 1);
        return Mathf.Round(input / stepSize) * stepSize;
    }

    private void HandleMovement(float verticalInput)
    {
        if (verticalInput != 0)
        {
            currentSpeed += verticalInput * accelerationSpeed * Time.deltaTime;
            float maxBackwardSpeed = accelerationSpeed * backwardSpeedLimit;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardSpeed, accelerationSpeed);

            if (AudioManager.instance.IsPlaying("Motor") == false)
            {
                AudioManager.instance.PlaySound("Motor");
            }
        }
        else
        {
            Decelerate();
            AudioManager.instance.StopSound("Motor");
        }

        MoveBoat();
    }

    private void Decelerate()
    {
        if (currentSpeed > 0)
        {
            currentSpeed -= decelerationSpeed * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);
        }
        else if (currentSpeed < 0)
        {
            currentSpeed += decelerationSpeed * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, 0);
        }
    }

    private void MoveBoat()
    {
        Vector3 forward = (boatFront.transform.position - transform.position).normalized;
        Vector3 newPosition = transform.position + forward * Time.deltaTime * currentSpeed;
        newPosition.y = transform.position.y; // Lock the Y-axis position
        transform.position = newPosition;
    }

}
