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
    public TextMeshProUGUI collectedText;


    public void SetFishData(FishData fishData)
    {
        this.fishData = fishData;
        fishImage.texture = fishData.UIIcon;
        fishName.text = fishData.fishName;

        // Using rich text tags to color only the rarity value
        string colorTag = GetColorForRarity(fishData.rarity);
        rarityText.text = "<size=70%>Rarity</size>\n" + colorTag + fishData.rarity.ToString() + "</color>";

        habitatText.text = "<size=70%>Habitat</size>\n" + fishData.habitat.ToString();

        collectedText.text = "<size=70%>Collected</size>\n" + LevelManager.instance.playerCollection.GetFishCount(fishData.fishID);
    }

    private string GetColorForRarity(Rarity rarity)
    {
        Color color;
        switch (rarity)
        {
            case Rarity.Common:
                color = UIManager.instance.commonColor;
                break;
            case Rarity.Uncommon:
                color = UIManager.instance.uncommonColor;
                break;
            case Rarity.Rare:
                color = UIManager.instance.rareColor;
                break;
            case Rarity.Legendary:
                color = UIManager.instance.legendaryColor;
                break;
            default:
                color = new Color(0.5f, 0.5f, 0.5f);
                break;
        }

        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
    }
}