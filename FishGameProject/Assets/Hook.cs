using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody rb;
    public bool onWater = false;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnEnterWater()
    {
        onWater = true;
        rb.isKinematic = true;
        Vector3 currentPosition = transform.position;
        currentPosition.y = 0;
        transform.position = currentPosition;
    }
}
