using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("References")]
    [SerializeField] private Transform head;

    [Header("UI Canvases")]
    private Dictionary<CanvasEnum, GameObject> canvasMap;
    [SerializeField] private GameObject collectionCanvas;
    [SerializeField] private GameObject minigameCanvas;
    [HideInInspector] public CollectionCanvas collectionCanvasScript;
    [HideInInspector] public MinigameCanvas minigameCanvasScript;

    [Header("UI Canvas Settings")]
    [SerializeField] private UICanvasSettings collectionCanvasSettings;
    [SerializeField] private UICanvasSettings sliderMinigameCanvasSettings;
    [SerializeField] private UICanvasSettings beatFishMinigameCanvasSettings;
    private Vector3 desiredPosition;
    private Vector3 directionToFace;
    private Quaternion desiredRotation;

    private float initialCanvasY;

    private Dictionary<GameObject, float> initialYPositions = new Dictionary<GameObject, float>();


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

        canvasMap = new Dictionary<CanvasEnum, GameObject>()
        {
            { CanvasEnum.Collection, collectionCanvas },
            { CanvasEnum.Minigame, minigameCanvas }
        };

        collectionCanvasScript = collectionCanvas.GetComponent<CollectionCanvas>();
        minigameCanvasScript = minigameCanvas.GetComponent<MinigameCanvas>();

        initialYPositions[collectionCanvas] = collectionCanvas.transform.position.y;
        initialYPositions[minigameCanvas] = minigameCanvas.transform.position.y;
    }

    public void SetCanvasState(CanvasEnum canvasEnum, UIStateEnum state)
    {
        if (!canvasMap.TryGetValue(canvasEnum, out GameObject canvas))
        {
            Debug.LogError("Invalid CanvasEnum");
            return;
        }

        switch (state)
        {
            case UIStateEnum.Enable:
                MoveCanvasToInitialPosition(canvas, GetSettingsForCanvas(canvas));
                canvas.SetActive(true);
                initialYPositions[canvas] = canvas.transform.position.y;  // Set after moving to initial position
                break;
            case UIStateEnum.Disable:
                canvas.SetActive(false);
                break;
            case UIStateEnum.Toggle:
                if (!canvas.activeSelf)
                {
                    MoveCanvasToInitialPosition(canvas, GetSettingsForCanvas(canvas));
                }
                canvas.SetActive(!canvas.activeSelf);
                break;
            default:
                Debug.LogError("Invalid UIStateEnum");
                break;
        }

        initialCanvasY = canvas.transform.position.y;
    }

    void Update()
    {
        MoveCanvas(collectionCanvas, collectionCanvasSettings);
        MoveCanvas(minigameCanvas, GetCurrentMinigameSettings());
    }

    private void MoveCanvasToInitialPosition(GameObject canvas, UICanvasSettings settings)
    {
        Vector3 startPosition = head.position + head.forward * settings.spawnDistance + head.right * settings.rightOffset;
        canvas.transform.position = startPosition;
        Vector3 directionToFace = (head.position - canvas.transform.position).normalized;
        Quaternion startRotation = Quaternion.LookRotation(directionToFace) * Quaternion.Euler(0, 180, 0);
        canvas.transform.rotation = startRotation;
    }

    private UICanvasSettings GetSettingsForCanvas(GameObject canvas)
    {
        if (canvas == collectionCanvas)
            return collectionCanvasSettings;
        else if (canvas == minigameCanvas)
            return GetCurrentMinigameSettings();

        return null;
    }

    private UICanvasSettings GetCurrentMinigameSettings()
    {
        switch (LevelManager.instance.currentMinigame)
        {
            case MinigameEnum.Slider:
                return sliderMinigameCanvasSettings;
            case MinigameEnum.BeatFish:
                return beatFishMinigameCanvasSettings;
            default:
                return null;
        }
    }

    private void MoveCanvas(GameObject canvas, UICanvasSettings settings)
    {
        if (!canvas.activeSelf) return;

        desiredPosition = head.position + head.forward * settings.spawnDistance + head.right * settings.rightOffset;
        if (settings.limitYRange)
        {
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, initialYPositions[canvas] - settings.minY, initialYPositions[canvas] + settings.maxY);
        }


        directionToFace = (head.position - canvas.transform.position).normalized;
        desiredRotation = Quaternion.LookRotation(directionToFace) * Quaternion.Euler(0, 180, 0);

        if (Vector3.Distance(canvas.transform.position, desiredPosition) > settings.positionTolerance)
        {
            canvas.transform.position = Vector3.Lerp(canvas.transform.position, desiredPosition, settings.positionLerpSpeed * Time.deltaTime);
            canvas.transform.rotation = Quaternion.Slerp(canvas.transform.rotation, desiredRotation, settings.rotationLerpSpeed * Time.deltaTime);
        }
    }
}
