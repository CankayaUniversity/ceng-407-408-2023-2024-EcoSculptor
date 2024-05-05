using System;
using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Light DirectionalLight;
    //[SerializeField] private LightingPreset Preset;      Mevsimler için kullanılabilir

    [Header("Variables")] 
    [SerializeField, Range(0, 300f)] private float TimeOfDay = 100;

    [Header("Textures")] 
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;

    private void Start()
    {
        RenderSettings.skybox.SetTexture("_Texture1", skyboxDay);
        RenderSettings.skybox.SetFloat("_Blend", 0);

        //Debug.Log(RenderSettings.skybox.GetTexture("_Texture1").name);
        //Debug.Log("skyboxNight: "+ skyboxNight.name + " skyboxSunrise: "+ skyboxSunrise.name + 
                 // " skyboxDay: "+ skyboxDay.name + " skyboxSunset: "+ skyboxSunset.name);
    }

    private void Update()
    {
        // if(!Preset)
        //     return;
        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 300f;
            UpdateLighting(TimeOfDay / 300f);
            ChangeSkybox();
        }
        else
        {
            UpdateLighting(TimeOfDay / 300f);
        }

    }


    private void UpdateLighting(float timePercent)
    {
        //RenderSettings.ambientLight = Preset.ambientColor.Evaluate(timePercent);
        //RenderSettings.fogColor = Preset.fogColor.Evaluate(timePercent);

        if (DirectionalLight)
        {
            //DirectionalLight.color = Preset.directionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));

        }
    }

    private void ChangeSkybox()
    {
        if(TimeOfDay is >= 100.0f and < 105.0f && RenderSettings.skybox.GetTexture("_Texture1").name != skyboxDay.name)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 20f));
        }
        else if (TimeOfDay is >= 225.0f  and < 230.0f && RenderSettings.skybox.GetTexture("_Texture1").name != skyboxSunset.name)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));

        }
        else if(TimeOfDay is >= 250.0f and < 255.0f && RenderSettings.skybox.GetTexture("_Texture1").name != skyboxNight.name)
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 20f));

        }
        else if(TimeOfDay is >= 75.0f and < 80.0f && RenderSettings.skybox.GetTexture("_Texture1").name != skyboxSunrise.name)
        {
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 10f));
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

    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);

        for (float i = 0; i < time; i+= Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
        
        
    }
    
}
