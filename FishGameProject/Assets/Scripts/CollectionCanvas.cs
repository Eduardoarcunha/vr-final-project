using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCanvas : MonoBehaviour
{
    public Panel collectionPanel;
    public FishInfoPanel fishInfoPanel;

    public void OpenCollectionPanel()
    {
        collectionPanel.OpenPanel();
    }

    public void CloseCollectionPanel()
    {
        collectionPanel.ClosePanel();
    }

    public void OpenFishInfoPanel(FishData fishData)
    {
        fishInfoPanel.SetFishData(fishData);
        fishInfoPanel.OpenPanel();
    }

    public void CloseFishInfoPanel()
    {
        fishInfoPanel.ClosePanel();
    }

    public void ReturnToCollection()
    {
        CloseFishInfoPanel();
        collectionPanel.OpenPanel();
    }
}
