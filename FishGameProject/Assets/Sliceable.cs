using EzySlice;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    public Material crossSectionMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fishing Rod"))
        {
            Vector3 pointOfContact = other.ClosestPointOnBounds(transform.position);
            Vector3 sliceNormal = other.transform.up;

            Vector3 contactDirection = (pointOfContact - transform.position).normalized;

            string slicedFace = DetermineSlicedFace(contactDirection);

            SlicedHull slicedObject = gameObject.Slice(pointOfContact, sliceNormal, crossSectionMaterial);
            if (slicedObject != null)
            {
                GameObject upperPart = slicedObject.CreateUpperHull(gameObject, crossSectionMaterial);
                GameObject lowerPart = slicedObject.CreateLowerHull(gameObject, crossSectionMaterial);

                upperPart.AddComponent<MeshCollider>().convex = true;
                lowerPart.AddComponent<MeshCollider>().convex = true;

                upperPart.AddComponent<Rigidbody>();
                lowerPart.AddComponent<Rigidbody>();

                gameObject.SetActive(false);

                Debug.Log("Correct slice on face: " + slicedFace);
            }
        }
    }

    private string DetermineSlicedFace(Vector3 contactDirection)
    {
        Vector3[] faceNormals = {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        string[] faceNames = { "Top", "Bottom", "Left", "Right", "Front", "Back" };

        float maxDot = float.MinValue;
        int faceIndex = -1;

        for (int i = 0; i < faceNormals.Length; i++)
        {
            float dot = Vector3.Dot(contactDirection, faceNormals[i]);
            if (Mathf.Abs(dot) > maxDot)
            {
                maxDot = Mathf.Abs(dot);
                faceIndex = i;
            }
        }

        return faceNames[faceIndex];
    }
}
