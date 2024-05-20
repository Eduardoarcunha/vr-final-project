using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private Material SkyboxMaterial;

    [Header("Time of Day")]
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField, Range(0.05f, 10f)] private float TimeMultiplier;

    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    private static readonly int Exposure = Shader.PropertyToID("_Exposure");

    private float cumulativeRotation = 0f;

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            // Increment time by deltaTime multiplied by the TimeMultiplier
            TimeOfDay += Time.deltaTime * TimeMultiplier;
            TimeOfDay %= 24; // Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
            UpdateSkybox(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
            UpdateSkybox(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        // Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        // If the directional light is set then rotate and set its color
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f), 170f, 0));
        }
    }

    private void UpdateSkybox(float timePercent)
    {
        if (SkyboxMaterial != null)
        {
            float rotationSpeed = 2f;
            cumulativeRotation += Time.deltaTime * TimeMultiplier * rotationSpeed;
            SkyboxMaterial.SetFloat(Rotation, cumulativeRotation);

            float exposure = Mathf.Clamp01(Mathf.Cos(timePercent * Mathf.PI * 2 - Mathf.PI) * 0.5f + 0.5f);
            SkyboxMaterial.SetFloat(Exposure, exposure);
        }
    }

    // Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        // Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        // Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
