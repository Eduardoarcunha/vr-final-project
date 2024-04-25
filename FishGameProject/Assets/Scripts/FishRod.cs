using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class FishRod : MonoBehaviour
{
    [Header("Fish Line")]
    public int totalLinePoints;
    public LineRenderer lineRenderer;

    [Header("Reel Knob")]
    public GameObject reelGameObject;
    public float reelKnobTreshold;
    private XRKnob reelKnob;
    private float lastKnobValue;

    [Header("Hook")]
    public GameObject hookGameObject;
    public Transform hookReturnPosition;

    private Transform hookTransform;
    private Hook hook;
    private Rigidbody hookRigidbody;
    private bool isHookThrown = false;
    private Vector3 previousHookPosition;
    private Vector3 currentHookVelocity;
    private float throwForce;

    [Header("Other References")]
    public Transform rodTipTransform;
    public Transform rodMidTransform;
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;
    private XRGrabInteractable grabInteractable;


    void Awake()
    {
        reelKnob = reelGameObject.GetComponent<XRKnob>();
        hook = hookGameObject.GetComponent<Hook>();
        hookTransform = hookGameObject.transform;
        hookRigidbody = hookGameObject.GetComponent<Rigidbody>();

        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x => ThrowHook());
    }

    void Start()
    {
        previousHookPosition = hookRigidbody.position;
        lastKnobValue = reelKnob.value;
    }

    void Update()
    {

        UpdateLinePositions();

        if (Mathf.Abs(reelKnob.value - lastKnobValue) > reelKnobTreshold)
        {
            RotateFishingRod(reelKnob.value - lastKnobValue);
            lastKnobValue = reelKnob.value;
        }

        CalculateThrowForce();
        if (isHookThrown) ReelInHook();
    }


    void UpdateLinePositions()
    {
        lineRenderer.positionCount = totalLinePoints;

        Vector3 reelPosition = reelGameObject.transform.position;
        Vector3 rodMidPosition = rodMidTransform.position;
        Vector3 rodTipPosition = rodTipTransform.position;
        Vector3 hookPosition = hookTransform.position;

        // Define the proportion of points for each segment based on their expected complexity
        int pointsReelToMid = Mathf.RoundToInt(totalLinePoints * 0.25f);
        int pointsMidToTip = Mathf.RoundToInt(totalLinePoints * 0.25f);
        int pointsTipToHook = totalLinePoints - pointsReelToMid - pointsMidToTip;

        int pointIndex = 0;  // To keep track of the current point index

        // Segment: Reel to Rod Mid
        for (int i = 0; i < pointsReelToMid; i++)
        {
            float t = (float)i / (pointsReelToMid - 1);
            Vector3 position = Vector3.Lerp(reelPosition, rodMidPosition, t);
            lineRenderer.SetPosition(pointIndex++, position);
        }

        // Segment: Rod Mid to Rod Tip
        for (int i = 0; i < pointsMidToTip; i++)
        {
            float t = (float)i / (pointsMidToTip - 1);
            Vector3 position = Vector3.Lerp(rodMidPosition, rodTipPosition, t);
            lineRenderer.SetPosition(pointIndex++, position);
        }

        if (isHookThrown)
        {
            for (int i = 0; i < pointsTipToHook; i++)
            {
                float t = (float)i / (pointsTipToHook - 1);
                Vector3 position = CalculateBezierPoint(t, rodTipPosition, GetControlPoint(rodTipPosition, hookPosition), hookPosition);
                lineRenderer.SetPosition(pointIndex++, position);
            }
        }
        else
        {
            // Clear the points or retract to the rod tip if the hook has not been thrown
            for (int i = 0; i < pointsTipToHook; i++)
            {
                lineRenderer.SetPosition(pointIndex++, rodTipPosition); // Set to rod tip position to "hide" it
            }
        }
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // First term
        p += 2 * u * t * p1; // Second term
        p += tt * p2; // Third term

        return p;
    }

    Vector3 GetControlPoint(Vector3 rodTip, Vector3 hook)
    {
        Vector3 midPoint = (rodTip + hook) / 2;
        Vector3 controlPoint = midPoint + new Vector3(0, -1, 0);
        return controlPoint;
    }


    void CalculateThrowForce()
    {
        Vector3 hookPosition = hookRigidbody.position;
        Vector3 velocity = (hookPosition - previousHookPosition) / Time.deltaTime;

        throwForce = CalculateForceBasedOnVelocityChange(velocity - currentHookVelocity);

        previousHookPosition = hookPosition;
        currentHookVelocity = velocity;
    }

    private float CalculateForceBasedOnVelocityChange(Vector3 velocityChange)
    {
        float magnitude = velocityChange.magnitude;
        if (magnitude <= 10) return 2;
        if (magnitude <= 60) return 8;
        return 10;
    }


    void ThrowHook()
    {
        if (isHookThrown) return;
        hookRigidbody.isKinematic = false;
        hookRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        isHookThrown = true;
    }

    void RotateFishingRod(float knobDelta)
    {
        if (knobDelta < 0)
        {
            Vector3 targetPositionXZ = new Vector3(hookReturnPosition.position.x, hookTransform.position.y, hookReturnPosition.position.z);
            float distanceXZ = Vector3.Distance(new Vector2(hookTransform.position.x, hookTransform.position.z),
                                                new Vector2(hookReturnPosition.position.x, hookReturnPosition.position.z));

            float speedFactor = Mathf.Max(0.5f, 5f - 4.5f * (distanceXZ / Vector3.Distance(hookReturnPosition.position, previousHookPosition)));
            if (distanceXZ > 1f) // Prefer horizontal movement first
            {
                hookTransform.position = Vector3.MoveTowards(hookTransform.position, targetPositionXZ, speedFactor * Time.deltaTime);
            }
            else
            {
                hookTransform.position = Vector3.MoveTowards(hookTransform.position, hookReturnPosition.position, speedFactor * Time.deltaTime);
            }

            AudioManager.instance.PlaySound("FishRod");
        }
        else if (knobDelta > 0)
        {
            AudioManager.instance.PlaySound("FishRod");
        }
        else
        {
            AudioManager.instance.StopSound("FishRod");
        }
    }


    void ReelInHook()
    {
        float distanceToRodTip = Vector3.Distance(hookRigidbody.position, hookReturnPosition.position);
        if ((hook.onFloor || hook.onWater) && Mathf.Abs(distanceToRodTip) <= 1.5f)
        {
            hookRigidbody.isKinematic = true;
            hookGameObject.transform.parent = gameObject.transform;
            hookTransform.position = hookReturnPosition.position;
            isHookThrown = false;
            hook.onWater = false;
            hook.onFloor = false;
            AudioManager.instance.PlaySound("PullHook");
        }
    }
}
