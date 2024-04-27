using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Transform head;
    [SerializeField] private float spawnDistance;
    [SerializeField] private GameObject collectionCanvas;
    [SerializeField] private GameObject minigameCanvas;
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

    public void SetCollectionCanvasState(UIStateEnum state)
    {
        switch (state)
        {
            case UIStateEnum.Enable:
                collectionCanvas.SetActive(true);
                break;
            case UIStateEnum.Disable:
                collectionCanvas.SetActive(false);
                break;
            case UIStateEnum.Toggle:
                collectionCanvas.SetActive(!collectionCanvas.activeSelf);
                break;
        }
    }

    public void SetMinigameCanvasState(UIStateEnum state)
    {
        switch (state)
        {
            case UIStateEnum.Enable:
                minigameCanvas.SetActive(true);
                break;
            case UIStateEnum.Disable:
                minigameCanvasScript.StopAllCoroutines();
                minigameCanvas.SetActive(false);
                break;
            case UIStateEnum.Toggle:
                minigameCanvas.SetActive(!minigameCanvas.activeSelf);
                break;
        }
    }

    public void SetSliderValue(float value, SliderEnum sliderEnum)
    {
        if (sliderEnum == SliderEnum.Player || sliderEnum == SliderEnum.Fish)
        {
            minigameCanvasScript.SetSliderValue(value, sliderEnum);
        }
        else
        {
            Debug.LogError("Invalid SliderEnum");
        }

    }

    public void AddSliderValue(float value, SliderEnum slideEnum)
    {
        if (slideEnum == SliderEnum.Player || slideEnum == SliderEnum.Fish)
        {
            minigameCanvasScript.AddSliderValue(value, slideEnum);
        }
        else
        {
            Debug.LogError("Invalid SliderEnum");
        }
    }

    public float GetSliderValue(SliderEnum sliderEnum)
    {
        if (sliderEnum == SliderEnum.Player || sliderEnum == SliderEnum.Fish)
        {
            return minigameCanvasScript.GetSliderValue(sliderEnum);
        }
        else
        {
            Debug.LogError("Invalid SliderEnum");
            return -1;
        }
    }

    public void ColorFishBackground(Color color)
    {
        minigameCanvasScript.ColorFishBackground(color);
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
