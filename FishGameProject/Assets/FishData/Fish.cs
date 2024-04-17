using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Fish", menuName = "Scriptables/Fish", order = 1)]
public class FishData : ScriptableObject
{
    public Texture2D UIIcon;
    public string fishName;
}
