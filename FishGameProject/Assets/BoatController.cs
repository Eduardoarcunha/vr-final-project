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

    void Update()
    {
        // float verticalInput = Input.GetAxis("Vertical");
        // float horizontalInput = Input.GetAxis("Horizontal");

        float verticalInput = engineSlider.value * 2 - 1;
        float horizontalInput = steeringKnob.value * 2 - 1;


        if (verticalInput != 0)
        {
            currentSpeed += verticalInput * accelerationSpeed * Time.deltaTime;
            float maxBackwardSpeed = accelerationSpeed * backwardSpeedLimit;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardSpeed, accelerationSpeed);
        }
        else
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

        Vector3 forward = boatFront.transform.position - transform.position;
        transform.position += forward * Time.deltaTime * currentSpeed;
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

    }
}