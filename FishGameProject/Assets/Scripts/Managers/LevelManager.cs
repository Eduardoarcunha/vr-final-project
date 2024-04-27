using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private bool isMiniGameActive = false;
    private Coroutine miniGameCoroutine;

    [SerializeField] private float sliderMinigameSpeedFactor;
    [SerializeField] private Color sliderMinigameCloseColor;
    [SerializeField] private Color sliderMinigameFarColor;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AudioManager.instance.PlaySound("OceanSound");
    }

    void Update()
    {
        if (isMiniGameActive)
        {
            if (miniGameCoroutine == null)
            {
                miniGameCoroutine = StartCoroutine(ChangeSliderValueOverTime());
            }

            CheckSlidersDistance();
        }
    }

    public void StartMiniGame()
    {
        isMiniGameActive = true;
        UIManager.instance.SetMinigameCanvasState(UIStateEnum.Enable);
        miniGameCoroutine = StartCoroutine(ChangeSliderValueOverTime());
    }

    public void EndMiniGame()
    {
        isMiniGameActive = false;
        if (miniGameCoroutine != null)
        {
            StopCoroutine(miniGameCoroutine);
            miniGameCoroutine = null;
        }
        UIManager.instance.SetMinigameCanvasState(UIStateEnum.Disable);
    }

    private IEnumerator ChangeSliderValueOverTime()
    {
        float targetValue = Random.Range(0f, 1f);
        float initialValue = UIManager.instance.GetSliderValue(SliderEnum.Fish);
        float distance = Mathf.Abs(targetValue - initialValue);

        float dynamicDuration = distance * sliderMinigameSpeedFactor;

        float elapsedTime = 0;

        while (elapsedTime < dynamicDuration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(initialValue, targetValue, elapsedTime / dynamicDuration);
            UIManager.instance.SetSliderValue(newValue, SliderEnum.Fish);
            yield return null;
        }
        UIManager.instance.SetSliderValue(targetValue, SliderEnum.Fish);
        miniGameCoroutine = null;
    }

    public void AddSliderValue(float value, SliderEnum sliderEnum)
    {
        UIManager.instance.AddSliderValue(value, sliderEnum);
    }

    public void CheckSlidersDistance()
    {
        float playerValue = UIManager.instance.GetSliderValue(SliderEnum.Player);
        float fishValue = UIManager.instance.GetSliderValue(SliderEnum.Fish);

        if (Mathf.Abs(playerValue - fishValue) < 0.1f)
        {
            UIManager.instance.ColorFishBackground(sliderMinigameCloseColor);
        }
        else
        {
            UIManager.instance.ColorFishBackground(sliderMinigameFarColor);
        }
    }

}
