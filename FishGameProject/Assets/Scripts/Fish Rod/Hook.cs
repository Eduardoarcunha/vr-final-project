using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody rb;
    public bool onFloor = false;
    public bool onWater = false;
    public GameObject fishPrefab;

    [Header("Fish Launch Settings")]
    [SerializeField] private float launchPower = 10f;
    [SerializeField] private float heightFactor = 2f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Fish Spot") && !onWater && !onFloor && LevelManager.instance.currentMinigame == MinigameEnum.None && LevelManager.instance.fishRod.isHookThrown)
        {
            AudioManager.instance.PlaySound("WaterSplash");
            onWater = true;
            FreezeHook();
            FishSpot fishSpot = other.GetComponent<FishSpot>();
            FishData fish = LevelManager.instance.fishCollection.GetRandomFishByHabitat(fishSpot.habitat);
            LevelManager.instance.StartMiniGame(fish);
        }
        else if (other.CompareTag("Water") && !onWater && !onFloor && LevelManager.instance.currentMinigame == MinigameEnum.None && LevelManager.instance.fishRod.isHookThrown)
        {
            AudioManager.instance.PlaySound("WaterSplash");
            onWater = true;
            FreezeHook();
            FishData fish = LevelManager.instance.fishCollection.GetRandomFish();
            LevelManager.instance.StartMiniGame(fish);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onFloor = true;
            FreezeHook();
        }
    }

    private void FreezeHook()
    {
        rb.isKinematic = true;
        Vector3 currentPosition = transform.position;
        transform.parent = null;
        currentPosition.y = 0;
        transform.position = currentPosition;
    }

    public void LaunchFish()
    {
        GameObject fish = Instantiate(fishPrefab, transform.position, Quaternion.identity);
        Rigidbody fishRigidbody = fish.GetComponent<Rigidbody>();

        Vector3 targetDirection = UIManager.instance.head.position - transform.position;
        float distance = targetDirection.magnitude;
        Vector3 targetWithHeight = targetDirection + Vector3.up * distance * heightFactor;

        fishRigidbody.AddForce(targetWithHeight.normalized * launchPower, ForceMode.VelocityChange);
    }
}
