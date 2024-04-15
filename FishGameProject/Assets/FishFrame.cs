using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishFrame : MonoBehaviour
{
    public FishData fishData;
    public RawImage fishImage;
    public TextMeshProUGUI fishName;

    void Start()
    {
        fishImage.texture = fishData.UIIcon;
        fishName.text = fishData.fishName;
    }

}
