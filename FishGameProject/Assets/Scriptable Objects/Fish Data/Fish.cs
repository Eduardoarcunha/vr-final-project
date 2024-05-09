using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

public enum Habitat
{
    DrownedForest,
    AridWaterways,
    CoralReef,
}

[CreateAssetMenu(fileName = "FishData", menuName = "ScriptableObjects/FishData", order = 1)]
public class FishData : ScriptableObject
{


    public Texture2D UIIcon;
    public string fishName;
    public Rarity rarity;
    public Habitat habitat;

}
