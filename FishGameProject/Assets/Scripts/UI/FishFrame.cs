using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;
using TMPro;

public class FishFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CollectionCanvas collectionCanvas;

    [Header("Fish Data")]
    public FishData fishData;

    [Header("UI Elements")]
    public RawImage fishImage;
    public TextMeshProUGUI fishName;
    public CanvasGroup canvasGroup;

    [Header("Scale and Alpha Settings")]
    public Vector3 normalScale;
    public Vector3 hoveredScale;

    private float startAlpha;
    public float uncollectedAlpha;
    public float collectedAlpha;
    public float hoveredAlpha;

    private bool isHovering = false;

    private float scaleSpeed = 4f; // Adjust the speed of the scale transition

    void Start()
    {
        fishImage.texture = fishData.UIIcon;
        fishName.text = fishData.fishName;

        transform.localScale = normalScale;
        startAlpha = LevelManager.instance.playerCollection.HasFish(fishData.fishID) ? collectedAlpha : uncollectedAlpha;
        canvasGroup.alpha = startAlpha;
    }

    void OnEnable()
    {
        transform.localScale = normalScale;
        startAlpha = LevelManager.instance.playerCollection.HasFish(fishData.fishID) ? collectedAlpha : uncollectedAlpha;
        canvasGroup.alpha = startAlpha;
    }

    void Update()
    {
        if (isHovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, hoveredScale, Time.deltaTime * scaleSpeed);
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, hoveredAlpha, Time.deltaTime * scaleSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, normalScale, Time.deltaTime * scaleSpeed);
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, startAlpha, Time.deltaTime * scaleSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        collectionCanvas.CloseCollectionPanel();
        collectionCanvas.OpenFishInfoPanel(fishData);
    }
}
