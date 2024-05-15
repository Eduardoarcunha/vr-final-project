using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCollection : MonoBehaviour
{
    public List<FishData> fishDataList = new List<FishData>();

    public void AddFish(FishData fish)
    {
        if (!fishDataList.Contains(fish))
        {
            fishDataList.Add(fish);
        }
    }


    public FishData GetRandomFish()
    {
        return GetWeightedRandomFish(fishDataList);
    }

    public FishData GetRandomFishByHabitat(Habitat habitat)
    {
        List<FishData> filteredFish = new List<FishData>();

        foreach (var fish in fishDataList)
        {
            if (fish.habitat == habitat)
            {
                filteredFish.Add(fish);
            }
        }

        if (filteredFish.Count > 0)
        {
            return GetWeightedRandomFish(filteredFish);
        }
        else
        {
            return null;
        }
    }

    private FishData GetWeightedRandomFish(List<FishData> fishes)
    {
        List<FishData> weightedList = new List<FishData>();

        foreach (var fish in fishes)
        {
            int count = 1;
            switch (fish.rarity)
            {
                case Rarity.Common:
                    count = 4;
                    break;
                case Rarity.Uncommon:
                    count = 3;
                    break;
                case Rarity.Rare:
                    count = 2;
                    break;
                case Rarity.Legendary:
                    count = 1;
                    break;
            }

            for (int i = 0; i < count; i++)
            {
                weightedList.Add(fish);
            }
        }

        return weightedList[Random.Range(0, weightedList.Count)];
    }
}
