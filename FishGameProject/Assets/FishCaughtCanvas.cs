using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishCaughtCanvas : MonoBehaviour
{
    [SerializeField] private RawImage fishImage;
    [SerializeField] private TextMeshProUGUI fishName;

    void OnEnable()
    {
        FishData fishData = LevelManager.instance.currentFish;
        fishImage.texture = fishData.UIIcon;
        fishName.text = fishData.fishName;

        switch (fishData.rarity)
        {
            case Rarity.Common:
                fishName.color = UIManager.instance.commonColor;
                break;
            case Rarity.Uncommon:
                fishName.color = UIManager.instance.uncommonColor;
                break;
            case Rarity.Rare:
                fishName.color = UIManager.instance.rareColor;
                break;
            case Rarity.Legendary:
                fishName.color = UIManager.instance.legendaryColor;
                break;
            default:
                fishName.color = Color.white;
                break;
        }


        gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}