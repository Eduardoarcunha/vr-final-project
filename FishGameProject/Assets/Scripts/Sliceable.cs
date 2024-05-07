using EzySlice;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    public Material crossSectionMaterial;
    [SerializeField] private Vector3 requiredSliceDirection; // Direction in which the cube should be sliced
    public GameObject[] indicators;
    public Transform[] faceTransforms;

    void Start()
    {
        foreach (GameObject indicator in indicators)
        {
            indicator.SetActive(false);
        }

        int randomIndex = Random.Range(0, 4);
        if (randomIndex == 0) // TopBottom
            requiredSliceDirection = faceTransforms[2].position - faceTransforms[0].position;
        else if (randomIndex == 1) // RightLeft
            requiredSliceDirection = faceTransforms[3].position - faceTransforms[1].position;
        else if (randomIndex == 2) // BottomTop
            requiredSliceDirection = faceTransforms[0].position - faceTransforms[2].position;
        else if (randomIndex == 3) // LeftRight
            requiredSliceDirection = faceTransforms[1].position - faceTransforms[3].position;

        requiredSliceDirection.Normalize();

        indicators[randomIndex].SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fishing Rod"))
        {
            FishRod fishRod = other.GetComponentInParent<FishRod>();
            Vector3 fishVelocity = fishRod.GetVelocity();
            Vector3 slicePlane = fishRod.GetSlicePlane();
            Vector3 pointOfContact = other.ClosestPoint(transform.position);

            // Perform the slicing
            SlicedHull slicedObject = gameObject.Slice(pointOfContact, slicePlane, crossSectionMaterial);
            if (slicedObject != null)
            {
                GameObject upperPart = slicedObject.CreateUpperHull(gameObject, crossSectionMaterial);
                GameObject lowerPart = slicedObject.CreateLowerHull(gameObject, crossSectionMaterial);

                upperPart.AddComponent<MeshCollider>().convex = true;
                lowerPart.AddComponent<MeshCollider>().convex = true;

                upperPart.AddComponent<Rigidbody>();
                lowerPart.AddComponent<Rigidbody>();

                gameObject.SetActive(false);

                if (IsCorrectDirection(fishVelocity, requiredSliceDirection))
                {
                    Debug.Log("Correct direction");
                }
                else
                {
                    Debug.Log("Incorrect direction");
                }

            }
        }
    }

    private bool IsCorrectDirection(Vector3 incomingDirection, Vector3 requiredDirection, float toleranceDegrees = 45f)
    {
        float angle = Vector3.Angle(incomingDirection.normalized, requiredDirection.normalized);

        Debug.Log("Required direction: " + requiredDirection.normalized);
        Debug.Log("Incoming direction: " + incomingDirection.normalized);
        Debug.Log("Angle: " + angle);

        return angle <= toleranceDegrees;
    }

}
