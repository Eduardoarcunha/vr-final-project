using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float accelerationSpeed = 5.0f;
    public float decelerationSpeed = 5.0f;
    public float turnSpeed = 100.0f;
    private float currentSpeed = 0.0f;

    public GameObject boatFront;

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical"); // W and S or Up and Down Arrow
        float horizontalInput = Input.GetAxis("Horizontal"); // A and D or Left and Right Arrow

        if (verticalInput != 0)
        {
            currentSpeed += verticalInput * accelerationSpeed * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -accelerationSpeed, accelerationSpeed);
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