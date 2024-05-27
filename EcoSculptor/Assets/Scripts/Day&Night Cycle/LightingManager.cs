using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Light DirectionalLight;
    //[SerializeField] private LightingPreset Preset;      Mevsimler için kullanılabilir

    [Header("Textures")] 
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;

    public static LightingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        RenderSettings.skybox.SetTexture("_Texture1", skyboxDay);
        RenderSettings.skybox.SetFloat("_Blend", 0);

        //Debug.Log(RenderSettings.skybox.GetTexture("_Texture1").name);
        //Debug.Log("skyboxNight: "+ skyboxNight.name + " skyboxSunrise: "+ skyboxSunrise.name + 
                 // " skyboxDay: "+ skyboxDay.name + " skyboxSunset: "+ skyboxSunset.name);
    }
    
    public void UpdateLighting(float timePercent)
    {
        //RenderSettings.ambientLight = Preset.ambientColor.Evaluate(timePercent);
        //RenderSettings.fogColor = Preset.fogColor.Evaluate(timePercent);

        if (DirectionalLight)
        {
            //DirectionalLight.color = Preset.directionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));

        }
    }

    public void ChangeSkybox()
    {
        if(TimeManager.Instance.CurrentTimeOfDay is >= 100.0f and < 105.0f && RenderSettings.skybox.GetTexture("_Texture1").name != skyboxDay.name)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 20f));
        }
        else if (TimeManager.Instance.CurrentTimeOfDay is >= 225.0f  and < 230.0f && RenderSettings.skybox.GetTexture("_Texture1").name != skyboxSunset.name)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));

        }
        else if(TimeManager.Instance.CurrentTimeOfDay is >= 250.0f and < 255.0f && RenderSettings.skybox.GetTexture("_Texture1").name != skyboxNight.name)
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 20f));

        }
        else if(TimeManager.Instance.CurrentTimeOfDay is >= 75.0f and < 80.0f && RenderSettings.skybox.GetTexture("_Texture1").name != skyboxSunrise.name)
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
