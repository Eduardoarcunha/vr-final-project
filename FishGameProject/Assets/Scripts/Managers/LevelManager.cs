using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

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
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySound("OceanSound");
    }

    public void StartMiniGame()
    {
        Debug.Log("Starting Mini Game");
        UIManager.instance.ToggleMinigameCanvas();
    }

    public void EndMiniGame()
    {
        Debug.Log("Ending Mini Game");
        UIManager.instance.ToggleMinigameCanvas();
    }

    public void ChangeSliderValue(float value)
    {
        UIManager.instance.AddSliderValue(value);
    }

}
