using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private float fishToPlayerMinDistance;
    [SerializeField] private float destroyTime;

    void Start()
    {
        StartCoroutine(DestroyFish());
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, UIManager.instance.head.position) < fishToPlayerMinDistance)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyFish()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
