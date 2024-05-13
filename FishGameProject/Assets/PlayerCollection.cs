using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollection : MonoBehaviour
{
    public List<int> collectedFish = new List<int>();

    public void AddFish(int fishID)
    {
        if (!collectedFish.Contains(fishID))
        {
            collectedFish.Add(fishID);
        }
    }

    public void RemoveFish(int fishID)
    {
        if (collectedFish.Contains(fishID))
        {
            collectedFish.Remove(fishID);
        }
    }

    public bool HasFish(int fishID)
    {
        return collectedFish.Contains(fishID);
    }
}
