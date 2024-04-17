using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCanvas : MonoBehaviour
{
    public FishInfoPanel fishInfoPanel;


    public void OpenFishInfoPanel(FishData fishData)
    {
        fishInfoPanel.SetFishData(fishData);
        fishInfoPanel.OpenPanel();
    }

    public void CloseFishInfoPanel()
    {
        fishInfoPanel.ClosePanel();
    }

}
