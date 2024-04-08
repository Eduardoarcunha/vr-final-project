using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFloater : MonoBehaviour
{
    public float maxXRotationAngle = 5f; // Max rotation angle around the X-axis
    public float maxYRotationAngle = 5f; // Max rotation angle around the Y-axis
    public float maxZRotationAngle = 5f; // Max rotation angle around the Z-axis
    public float changeInterval = 2f;    // Time in seconds between rotation changes

    private Quaternion targetRotation;
    private float timer;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // If the timer exceeds the interval, calculate a new rotation
        if (timer > changeInterval)
        {
            timer = 0f;
            targetRotation = GenerateRandomRotation();
        }

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / changeInterval);
    }

    Quaternion GenerateRandomRotation()
    {
        // Random rotation around each axis using respective max rotation angles
        float xRot = Random.Range(-maxXRotationAngle, maxXRotationAngle);
        float yRot = Random.Range(-maxYRotationAngle, maxYRotationAngle);
        float zRot = Random.Range(-maxZRotationAngle, maxZRotationAngle);

        return Quaternion.Euler(xRot, yRot, zRot) * transform.rotation;
    }
}
