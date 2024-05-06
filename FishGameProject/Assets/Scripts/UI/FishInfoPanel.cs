using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishInfoPanel : Panel
{
    public FishData fishData;

    [Header("UI Elements")]
    public RawImage fishImage;
    public TextMeshProUGUI fishName;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI habitatText;

    [Header("Color Settings")]
    public Color commonColor;
    public Color uncommonColor;
    public Color rareColor;
    public Color legendaryColor;

    public void SetFishData(FishData fishData)
    {
        this.fishData = fishData;
        fishImage.texture = fishData.UIIcon;
        fishName.text = fishData.fishName;

        // Using rich text tags to color only the rarity value
        string colorTag = GetColorForRarity(fishData.rarity);
        rarityText.text = "<size=70%>Rarity</size>\n" + colorTag + fishData.rarity.ToString() + "</color>";

        habitatText.text = "<size=70%>Habitat</size>\n" + fishData.habitat.ToString();
    }

    private string GetColorForRarity(Rarity rarity)
    {
        Color color;
        switch (rarity)
        {
            case Rarity.Common:
                color = commonColor;
                break;
            case Rarity.Uncommon:
                color = uncommonColor;
                break;
            case Rarity.Rare:
                color = rareColor;
                break;
            case Rarity.Legendary:
                color = legendaryColor;
                break;
            default:
                color = new Color(0.5f, 0.5f, 0.5f);
                break;
        }

        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
    }
}