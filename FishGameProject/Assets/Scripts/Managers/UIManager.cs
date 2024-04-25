using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Transform head;
    public float spawnDistance;
    public GameObject collectionCanvas;
    public GameObject minigameCanvas;
    private CollectionCanvas collectionCanvasScript;
    private MinigameCanvas minigameCanvasScript;

    public float positionLerpSpeed;
    public float rotationLerpSpeed;
    public float minigameOffset;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        collectionCanvasScript = collectionCanvas.GetComponent<CollectionCanvas>();
        minigameCanvasScript = minigameCanvas.GetComponent<MinigameCanvas>();
    }

    public void ToggleCollectionCanvas()
    {
        collectionCanvas.SetActive(!collectionCanvas.activeSelf);
    }

    public void ToggleMinigameCanvas()
    {
        minigameCanvas.SetActive(!minigameCanvas.activeSelf);
    }

    public void AddSliderValue(float value)
    {
        minigameCanvasScript.SetSliderValue(minigameCanvasScript.slider.value + value);
    }

    void Update()
    {
        if (collectionCanvas.activeSelf)
        {
            // Calculate desired position
            Vector3 desiredPosition = head.position + (head.forward * spawnDistance);

            // Calculate desired rotation to face towards the player
            Vector3 directionToFace = (head.position - collectionCanvas.transform.position).normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(directionToFace) * Quaternion.Euler(0, 180, 0); // Adjusting the rotation to face the player

            // Interpolate position and rotation to create a delay effect
            collectionCanvas.transform.position = Vector3.Lerp(collectionCanvas.transform.position, desiredPosition, positionLerpSpeed * Time.deltaTime);
            collectionCanvas.transform.rotation = Quaternion.Slerp(collectionCanvas.transform.rotation, desiredRotation, rotationLerpSpeed * Time.deltaTime);
        }


        if (minigameCanvas.activeSelf)
        {
            // Calculate desired position for minigameCanvas
            Vector3 desiredPosition = head.position + (head.forward * spawnDistance) + (head.right * minigameOffset);

            // Calculate desired rotation to face towards the player
            Vector3 directionToFace = (head.position - minigameCanvas.transform.position).normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(directionToFace) * Quaternion.Euler(0, 180, 0);

            // Interpolate position and rotation to create a delay effect
            minigameCanvas.transform.position = Vector3.Lerp(minigameCanvas.transform.position, desiredPosition, positionLerpSpeed * Time.deltaTime);
            minigameCanvas.transform.rotation = Quaternion.Slerp(minigameCanvas.transform.rotation, desiredRotation, rotationLerpSpeed * Time.deltaTime);
        }
    }
}
