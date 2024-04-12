using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public Transform head;
    public float spawnDistance;
    public GameObject collectionPanel;
    public InputActionProperty toggleCollectionPanel;

    public float positionLerpSpeed;
    public float rotationLerpSpeed;

    void Update()
    {
        // Toggle panel visibility
        if (toggleCollectionPanel.action.WasPressedThisFrame())
        {
            collectionPanel.SetActive(!collectionPanel.activeSelf);
        }

        if (collectionPanel.activeSelf)
        {
            // Calculate desired position
            Vector3 desiredPosition = head.position + (head.forward * spawnDistance);

            // Calculate desired rotation to face towards the player
            Vector3 directionToFace = (head.position - collectionPanel.transform.position).normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(directionToFace) * Quaternion.Euler(0, 180, 0); // Adjusting the rotation to face the player

            // Interpolate position and rotation to create a delay effect
            collectionPanel.transform.position = Vector3.Lerp(collectionPanel.transform.position, desiredPosition, positionLerpSpeed * Time.deltaTime);
            collectionPanel.transform.rotation = Quaternion.Slerp(collectionPanel.transform.rotation, desiredRotation, rotationLerpSpeed * Time.deltaTime);
        }
    }
}
