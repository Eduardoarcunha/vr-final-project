using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishInfoPanel : MonoBehaviour
{
    public FishData fishData;

    [Header("UI Elements")]
    public RawImage fishImage;
    public TextMeshProUGUI fishName;

    public void SetFishData(FishData fishData)
    {
        this.fishData = fishData;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        fishImage.texture = fishData.UIIcon;
        fishName.text = fishData.fishName;
        gameObject.SetActive(true);
    }
}
