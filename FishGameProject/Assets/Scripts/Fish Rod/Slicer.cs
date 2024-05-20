using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Slicer : MonoBehaviour
{
    [SerializeField] private FishRod fishRod;
    [SerializeField] private Material sliceMaterial;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slice Cube"))
        {
            Vector3 requiredSliceDirection = other.GetComponent<BeatCube>().GetRequiredSliceDirection();

            Vector3 slicePlane = fishRod.GetSlicePlane();
            Vector3 pointOfContact = other.ClosestPointOnBounds(transform.position);

            SlicedHull slicedObject = other.gameObject.Slice(pointOfContact, slicePlane, sliceMaterial);
            if (slicedObject != null)
            {
                AudioManager.instance.PlaySound("SliceCube");
                GameObject upperHull = slicedObject.CreateUpperHull(other.gameObject, sliceMaterial);
                GameObject lowerHull = slicedObject.CreateLowerHull(other.gameObject, sliceMaterial);

                upperHull.AddComponent<MeshCollider>().convex = true;
                lowerHull.AddComponent<MeshCollider>().convex = true;

                upperHull.AddComponent<Rigidbody>();
                lowerHull.AddComponent<Rigidbody>();


                // Call coroutine to destroy the object after 5 seconds
                StartCoroutine(DestroyObject(upperHull, 15f));
                StartCoroutine(DestroyObject(lowerHull, 15f));

                if (IsCorrectDirection(fishRod.currentFishRodVelocity, requiredSliceDirection))
                {
                    LevelManager.instance.CubeSliced(true);
                }
                else
                {
                    LevelManager.instance.CubeSliced(false);
                }
            }

            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            Vector3 slicePlane = fishRod.GetSlicePlane();
            Vector3 pointOfContact = collision.contacts[0].point;

            SlicedHull slicedObject = collision.gameObject.Slice(pointOfContact, slicePlane, sliceMaterial);
            if (slicedObject != null)
            {
                AudioManager.instance.PlaySound("SliceFish");
                GameObject upperHull = slicedObject.CreateUpperHull(collision.gameObject, sliceMaterial);
                GameObject lowerHull = slicedObject.CreateLowerHull(collision.gameObject, sliceMaterial);

                upperHull.AddComponent<MeshCollider>().convex = true;
                lowerHull.AddComponent<MeshCollider>().convex = true;

                upperHull.AddComponent<Rigidbody>();
                lowerHull.AddComponent<Rigidbody>();

                // Call coroutine to destroy the object after 5 seconds
                StartCoroutine(DestroyObject(upperHull, 15f));
                StartCoroutine(DestroyObject(lowerHull, 15f));
            }

            Destroy(collision.gameObject);
        }
    }

    private bool IsCorrectDirection(Vector3 incomingDirection, Vector3 requiredDirection, float threshold = 0.4f)
    {
        incomingDirection.Normalize();
        // Debug.Log("Incoming: " + incomingDirection);
        // Debug.Log("Required: " + requiredDirection);
        // Debug.Log(Vector3.Dot(incomingDirection, requiredDirection));
        float dotProduct = Vector3.Dot(incomingDirection, requiredDirection);
        return dotProduct > threshold;
    }

    private IEnumerator DestroyObject(GameObject obj, float time)
    {
        // Check if the object was already destroyed
        if (obj == null) yield break;
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}
