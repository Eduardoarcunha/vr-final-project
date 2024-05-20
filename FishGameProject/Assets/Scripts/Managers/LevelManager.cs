using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public MinigameEnum currentMinigame = MinigameEnum.None;
    private Coroutine miniGameCoroutine;

    [SerializeField] public FishRod fishRod;
    private Hook hook;
    private Transform head;
    public PlayerCollection playerCollection;
    public FishCollection fishCollection;

    [HideInInspector] public FishData currentFish;

    [Header("Slider Minigame Settings")]
    [SerializeField] private float sliderMinigameSpeedFactor;
    [SerializeField] private Color sliderMinigameCloseColor;
    [SerializeField] private Color sliderMinigameFarColor;
    [SerializeField] private float sliderTimerMax;
    private float sliderTimer;
    private float timeWithingCloseRange = 0;
    private float minWithingCloseRangePercentage = 0.1f;

    [Header("BeatFish Minigame Settings")]
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private int totalCubes;
    [SerializeField] private float cubeSpeed;
    [SerializeField] private float cubeSpawnDistance;
    [SerializeField] private float cubeSpawnDelay;
    private int spawnedCubes = 0;
    private int remainingCubes;
    private int correctSlices;
    private float correctSlicePercentage = 0.8f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        hook = fishRod.GetComponent<FishRod>().hookGameObject.GetComponent<Hook>();
    }

    void Start()
    {
        head = UIManager.instance.head;
        AudioManager.instance.PlaySound("OceanSound");
        ResetMiniGameSettings(null);
    }

    void ResetMiniGameSettings(FishData minigameFish)
    {
        currentFish = minigameFish;

        sliderTimer = sliderTimerMax;

        spawnedCubes = 0;
        remainingCubes = totalCubes;
        correctSlices = 0;
        UIManager.instance.minigameCanvasScript.SetBeatFishScore(correctSlices);
    }

    void Update()
    {
        if (currentMinigame == MinigameEnum.Slider)
        {
            CheckSlidersDistance();

            if (miniGameCoroutine == null)
            {
                miniGameCoroutine = StartCoroutine(ChangeFishSliderValueOverTime());
            }

            if (timeWithingCloseRange > sliderTimerMax * minWithingCloseRangePercentage)
            {
                EndMiniGame(true);
            }

            if (sliderTimer <= 0)
            {
                EndMiniGame(false);
            }

            sliderTimer -= Time.deltaTime;
        }
        else if (currentMinigame == MinigameEnum.BeatFish)
        {
            if (miniGameCoroutine == null && spawnedCubes < totalCubes)
            {
                miniGameCoroutine = StartCoroutine(SpawnCubesWithDelay());
            }

            if (remainingCubes == 0)
            {
                EndMiniGame(correctSlices / totalCubes >= correctSlicePercentage);
            }
        }
    }

    public void StartMiniGame(FishData minigameFish)
    {
        ResetMiniGameSettings(minigameFish);
        currentMinigame = (MinigameEnum)UnityEngine.Random.Range(0, 2);

        // Debug.Log("[MINIGAME] " + currentMinigame.ToString() + " started");
        // Debug.Log("[FISH] " + minigameFish.fishName);

        UIManager.instance.SetCanvasState(CanvasEnum.Minigame, UIStateEnum.Enable);
        if (currentMinigame == MinigameEnum.Slider)
        {
            StartSliderMinigame();
        }
        else if (currentMinigame == MinigameEnum.BeatFish)
        {
            StartBeatFishMinigame();
        }
    }

    public void StartSliderMinigame()
    {
        miniGameCoroutine = StartCoroutine(ChangeFishSliderValueOverTime());
        sliderTimer = sliderTimerMax;
        timeWithingCloseRange = 0;
    }

    public void StartBeatFishMinigame()
    {
        miniGameCoroutine = StartCoroutine(SpawnCubesWithDelay());
    }

    IEnumerator SpawnCubesWithDelay()
    {
        spawnedCubes++;
        float lateralOffset = 1.0f;

        bool spawnRight = UnityEngine.Random.Range(0, 2) == 0;
        Vector3 spawnPosition = head.position + (spawnRight ? 1 : -1) * head.right * lateralOffset + head.forward * cubeSpawnDistance;
        spawnPosition.y = 1.4f;
        GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        cube.GetComponent<BeatCube>().cubeSpeed = cubeSpeed;

        yield return new WaitForSeconds(cubeSpawnDelay);
        miniGameCoroutine = null;
    }

    public void EndMiniGame(bool win)
    {
        UIManager.instance.SetCanvasState(CanvasEnum.Minigame, UIStateEnum.Disable);

        if (miniGameCoroutine != null)
        {
            StopCoroutine(miniGameCoroutine);
            miniGameCoroutine = null;
        }

        currentMinigame = MinigameEnum.None;

        if (win)
        {
            if (currentFish != null)
            {
                playerCollection.AddFish(currentFish.fishID);
                UIManager.instance.SetCanvasState(CanvasEnum.FishCaught, UIStateEnum.Enable);
                AudioManager.instance.PlaySound("FishCaught");
            }
            hook.LaunchFish();
            currentFish = null;
        }
        else
        {
            Debug.Log("Lose");
        }


        fishRod.ReelInHook();
    }

    private IEnumerator ChangeFishSliderValueOverTime()
    {
        float targetValue = UnityEngine.Random.Range(0f, 1f);
        float initialValue = UIManager.instance.minigameCanvasScript.GetSliderValue(SliderEnum.Fish);
        float distance = Mathf.Abs(targetValue - initialValue);

        float dynamicDuration = distance * sliderMinigameSpeedFactor;

        float elapsedTime = 0;

        while (elapsedTime < dynamicDuration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(initialValue, targetValue, elapsedTime / dynamicDuration);
            UIManager.instance.minigameCanvasScript.SetSliderValue(newValue, SliderEnum.Fish);
            yield return null;
        }
        UIManager.instance.minigameCanvasScript.SetSliderValue(targetValue, SliderEnum.Fish);
        miniGameCoroutine = null;
    }

    public void AddSliderValue(float value, SliderEnum sliderEnum)
    {
        UIManager.instance.minigameCanvasScript.AddSliderValue(value, sliderEnum);
    }

    public void CheckSlidersDistance()
    {
        float playerValue = UIManager.instance.minigameCanvasScript.GetSliderValue(SliderEnum.Player);
        float fishValue = UIManager.instance.minigameCanvasScript.GetSliderValue(SliderEnum.Fish);

        if (Mathf.Abs(playerValue - fishValue) < 0.1f)
        {
            UIManager.instance.minigameCanvasScript.ColorFishBackground(sliderMinigameCloseColor);
            timeWithingCloseRange += Time.deltaTime;
        }
        else
        {
            UIManager.instance.minigameCanvasScript.ColorFishBackground(sliderMinigameFarColor);
        }
    }

    public void CubeSliced(bool correctDirection)
    {
        remainingCubes--;
        if (correctDirection)
        {
            correctSlices++;
            UIManager.instance.minigameCanvasScript.SetBeatFishScore(correctSlices);
        }
    }
}
