using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float waterLevel = 0.0f;
    public float floatHeight = 2.0f;  // How much of the boat should be above water
    public float bounceDamp = 0.05f;
    public Rigidbody rb;

    void FixedUpdate()
    {
        Vector3 actionPoint = transform.position + transform.up * -floatHeight;
        float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

        if (forceFactor > 0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);
            rb.AddForceAtPosition(uplift, actionPoint);
        }
    }
}
