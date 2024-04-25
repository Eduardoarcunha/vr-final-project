using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody rb;
    public bool onFloor = false;
    public bool onWater = false;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            onWater = true;
            rb.isKinematic = true;
            Vector3 currentPosition = transform.position;
            transform.parent = null;
            currentPosition.y = 0;
            transform.position = currentPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onFloor = true;
            rb.isKinematic = true;
            Vector3 currentPosition = transform.position;
            transform.parent = null;
            currentPosition.y = 0;
            transform.position = currentPosition;
        }
    }
}
