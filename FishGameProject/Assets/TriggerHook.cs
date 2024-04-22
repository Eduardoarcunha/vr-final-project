using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHook : MonoBehaviour
{
    public Rigidbody hook_rb;
    public Transform hook_t;
    public bool isHookWater = false;
    
    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hook"))
            {
                hook_rb.isKinematic = true;
                HookAgua();
                isHookWater = true;
            }
        }

    void Update()
    {
        if(isHookWater){
            HookAgua();
        }
    }

    void HookAgua(){
        Vector3 currentPosition = hook_t.position;
        currentPosition.y = 0;
        hook_t.position = currentPosition;
    }
}
