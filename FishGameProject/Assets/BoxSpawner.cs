using System.Collections;
using UnityEngine;

public class BoxGenerator : MonoBehaviour
{
    public GameObject boxPrefab; // Assign the box prefab with the indicator as a child
    public int numberOfBoxes = 5; // Total number of boxes to generate
    public float spawnInterval = 2f; // Time interval between spawns

    private void Start()
    {
        StartCoroutine(SpawnBoxes());
    }

    private IEnumerator SpawnBoxes()
    {
        for (int i = 0; i < numberOfBoxes; i++)
        {
            // Instantiate the box
            GameObject newBox = Instantiate(boxPrefab, RandomPosition(), Quaternion.identity);
            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
    }

}
