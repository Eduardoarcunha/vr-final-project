using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    StartMenu,
    Playing,
    Paused,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameState currentGameState;

    void Awake()
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

    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }
}
