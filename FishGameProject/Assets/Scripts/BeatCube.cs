using EzySlice;
using UnityEngine;

public class BeatCube : MonoBehaviour
{
    public float cubeSpeed;
    public Vector3 requiredSliceDirection;

    [SerializeField] private GameObject[] indicators;
    [SerializeField] private Transform[] faceTransforms;
    [SerializeField] private float cubeToPlayerMinDistance;

    private int randomIndex;


    void Start()
    {
        foreach (GameObject indicator in indicators)
        {
            indicator.SetActive(false);
        }

        randomIndex = Random.Range(0, 4);
        requiredSliceDirection.Normalize();
        indicators[randomIndex].SetActive(true);
    }

    public Vector3 GetRequiredSliceDirection()
    {
        if (randomIndex == 0) // TopBottom
            requiredSliceDirection = faceTransforms[2].position - faceTransforms[0].position;
        else if (randomIndex == 1) // RightLeft
            requiredSliceDirection = faceTransforms[3].position - faceTransforms[1].position;
        else if (randomIndex == 2) // BottomTop
            requiredSliceDirection = faceTransforms[0].position - faceTransforms[2].position;
        else if (randomIndex == 3) // LeftRight
            requiredSliceDirection = faceTransforms[1].position - faceTransforms[3].position;

        requiredSliceDirection.Normalize();
        return requiredSliceDirection;
    }

    void Update()
    {
        Vector3 downOffset = new Vector3(0, 0.5f, 0);
        Vector3 directionToFace = (LevelManager.instance.head.position - transform.position).normalized;
        Vector3 directionToFaceWithOffset = (LevelManager.instance.head.position - transform.position - downOffset).normalized;

        Quaternion desiredRotation = Quaternion.LookRotation(directionToFace);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 5f);

        Vector3 moveDirection = directionToFaceWithOffset;
        transform.position += moveDirection * cubeSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, LevelManager.instance.head.position) < cubeToPlayerMinDistance)
        {
            Destroy(gameObject);
        }
    }
}
