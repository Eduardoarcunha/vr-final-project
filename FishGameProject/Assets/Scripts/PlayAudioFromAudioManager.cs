using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioFromAudioManager : MonoBehaviour
{
    public string target;

    public void Play()
    {
        AudioManager.instance.PlaySound(target);
    }

    public void Play(string audioName)
    {
        AudioManager.instance.PlaySound(audioName);
    }
}
