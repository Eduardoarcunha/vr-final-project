using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFloater : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float maxXRotationAngle;
    // public float maxYRotationAngle; // This might still be shown for potential future use or remove if not needed at all.
    public float maxZRotationAngle;
    public float changeInterval = 2f;
    public float rotationSpeed = 0.5f;

    [Header("Bobbing Settings")]
    public float bobbingSpeed = 1f;
    public float bobbingAmount = 0.5f;

    private Quaternion targetRotation;
    private Quaternion startRotation;
    private float timer;
    private float startHeight;

    void Start()
    {
        startRotation = transform.rotation;
        targetRotation = GenerateRandomRotation();
        startHeight = transform.position.y;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Update rotation
        if (timer > changeInterval)
        {
            timer = 0f;
            startRotation = transform.rotation;
            targetRotation = GenerateRandomRotation();
        }

        // Smoothly rotate towards the target rotation, preserving Y-axis rotation
        float lerpFactor = Mathf.Clamp01(timer / changeInterval);
        Quaternion tempRotation = Quaternion.Lerp(startRotation, targetRotation, lerpFactor * rotationSpeed);
        transform.rotation = new Quaternion(tempRotation.x, startRotation.y, tempRotation.z, tempRotation.w);

        // Bobbing effect
        float newY = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount + startHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    Quaternion GenerateRandomRotation()
    {
        float xRot = Random.Range(-maxXRotationAngle, maxXRotationAngle);
        // Keep the current Y rotation
        float yRot = transform.rotation.eulerAngles.y;
        float zRot = Random.Range(-maxZRotationAngle, maxZRotationAngle);

        return Quaternion.Euler(xRot, yRot, zRot);
    }
}
