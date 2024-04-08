using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction
{
    public class XRSliderDiscrete : XRSlider
    {
        [SerializeField]
        [Tooltip("Discrete steps for the slider")]
        private float[] discreteSteps = new float[] { 0, 0.25f, 0.5f, 0.75f, 1.0f };

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
            // Adjust the slider position after deselecting
            AdjustToNearestStep();
        }

        private void AdjustToNearestStep()
        {
            this.value = GetNearestStep(this.value);
            // Call any additional logic if necessary
        }

        private float GetNearestStep(float value)
        {
            float closestStep = discreteSteps[0];
            float smallestDifference = Mathf.Abs(value - closestStep);

            foreach (float step in discreteSteps)
            {
                float difference = Mathf.Abs(value - step);
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    closestStep = step;
                }
            }

            return closestStep;
        }
    }
}
