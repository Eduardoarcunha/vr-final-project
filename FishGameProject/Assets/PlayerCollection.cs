using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollection : MonoBehaviour
{
    public Dictionary<int, int> collectedFish = new Dictionary<int, int>();

    public void AddFish(int fishID)
    {
        if (collectedFish.ContainsKey(fishID))
        {
            collectedFish[fishID]++;
        }
        else
        {
            collectedFish[fishID] = 1;
        }
    }

    public void RemoveFish(int fishID)
    {
        if (collectedFish.ContainsKey(fishID))
        {
            collectedFish[fishID]--;
            if (collectedFish[fishID] <= 0)
            {
                collectedFish.Remove(fishID);
            }
        }
    }

    public bool HasFish(int fishID)
    {
        return collectedFish.ContainsKey(fishID);
    }

    public int GetFishCount(int fishID)
    {
        if (collectedFish.ContainsKey(fishID))
        {
            return collectedFish[fishID];
        }
        return 0;
    }
}
