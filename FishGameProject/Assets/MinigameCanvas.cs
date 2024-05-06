using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameCanvas : MonoBehaviour
{
    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider fishSlider;
    [SerializeField] private Image fishBackground;
    [SerializeField] private float sliderSmoothDuration;

    private Coroutine playerSliderCoroutine;
    private Coroutine fishSliderCoroutine;

    public void ColorFishBackground(Color color)
    {
        fishBackground.color = color;
    }

    public float GetSliderValue(SliderEnum slider)
    {
        switch (slider)
        {
            case SliderEnum.Player:
                return playerSlider.value;
            case SliderEnum.Fish:
                return fishSlider.value;
            default:
                Debug.LogError("Invalid SliderEnum");
                return -1;
        }
    }

    public void SetSliderValue(float value, SliderEnum slider)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Attempted to set slider value while Minigame Canvas is inactive!");
            return;
        }

        switch (slider)
        {
            case SliderEnum.Player:
                if (playerSliderCoroutine != null)
                    StopCoroutine(playerSliderCoroutine);
                playerSliderCoroutine = StartCoroutine(SmoothSetValue(playerSlider, value, sliderSmoothDuration));
                break;
            case SliderEnum.Fish:
                if (fishSliderCoroutine != null)
                    StopCoroutine(fishSliderCoroutine);
                fishSliderCoroutine = StartCoroutine(SmoothSetValue(fishSlider, value, sliderSmoothDuration));
                break;
        }
    }

    public void AddSliderValue(float value, SliderEnum slider)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Attempted to add slider value while Minigame Canvas is inactive!");
            return;
        }

        float newValue = slider == SliderEnum.Player ? playerSlider.value + value : fishSlider.value + value;
        SetSliderValue(newValue, slider);
    }

    private IEnumerator SmoothSetValue(Slider targetSlider, float targetValue, float duration)
    {
        float startValue = targetSlider.value;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            targetSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetSlider.value = targetValue;
    }

}
