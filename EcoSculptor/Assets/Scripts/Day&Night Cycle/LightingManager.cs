using System;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [Header("Variables")] 
    [SerializeField, Range(0, 300)] private float TimeOfDay = 120;
    
    private void Update()
    {
        if(!Preset)
            return;
        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 300f;
            UpdateLighting(TimeOfDay / 300f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 300f);
        }
    }


    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.fogColor.Evaluate(timePercent);

        if (DirectionalLight)
        {
            DirectionalLight.color = Preset.directionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }
    
    
    
    private void OnValidate()
    {
        if(DirectionalLight)
            return;

        if (RenderSettings.sun)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (var light in lights)
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
