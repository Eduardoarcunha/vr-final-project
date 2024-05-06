using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [HideInInspector] public MinigameEnum currentMinigame = MinigameEnum.None;
    private Coroutine miniGameCoroutine;

    [SerializeField] private FishRod fishRod;

    [Header("Slider Minigame Settings")]
    [SerializeField] private float sliderMinigameSpeedFactor;
    [SerializeField] private Color sliderMinigameCloseColor;
    [SerializeField] private Color sliderMinigameFarColor;
    [SerializeField] private float sliderTimerMax;
    private float sliderTimer;
    private float timeWithingCloseRange = 0;
    private float minWithingCloseRangePercentage = 0.1f;



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
        sliderTimer = sliderTimerMax;
    }

    void Update()
    {
        if (currentMinigame == MinigameEnum.Slider)
        {
            if (sliderTimer <= 0 || timeWithingCloseRange > sliderTimerMax * minWithingCloseRangePercentage)
            {
                EndMiniGame();
            }
            if (miniGameCoroutine == null)
            {
                miniGameCoroutine = StartCoroutine(ChangeFishSliderValueOverTime());
            }

            CheckSlidersDistance();

            sliderTimer -= Time.deltaTime;
        }
    }

    public void StartMiniGame()
    {
        currentMinigame = (MinigameEnum)Random.Range(0, 0); // None cant be selected

        UIManager.instance.SetMinigameCanvasState(UIStateEnum.Enable);
        miniGameCoroutine = StartCoroutine(ChangeFishSliderValueOverTime());
        sliderTimer = sliderTimerMax;
        timeWithingCloseRange = 0;
    }

    public void EndMiniGame()
    {
        currentMinigame = MinigameEnum.None;
        UIManager.instance.SetMinigameCanvasState(UIStateEnum.Disable);

        if (miniGameCoroutine != null)
        {
            StopCoroutine(miniGameCoroutine);
            miniGameCoroutine = null;
        }

        if (timeWithingCloseRange / sliderTimerMax > minWithingCloseRangePercentage)
        {
            Debug.Log("You Win!");
        }
        else
        {
            Debug.Log("You Lose!");
        }

        fishRod.ReelInHook();
    }

    private IEnumerator ChangeFishSliderValueOverTime()
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
            timeWithingCloseRange += Time.deltaTime;
        }
        else
        {
            UIManager.instance.ColorFishBackground(sliderMinigameFarColor);
        }
    }

}
