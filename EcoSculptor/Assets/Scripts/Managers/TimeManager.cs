using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeManager : MonoBehaviour
{
    [SerializeField, Range(0, 300f)] private float currentTimeOfDay = 100;
    [SerializeField, Range(0, 1000f)] private float seasonTime = 900f;
    
    public float totalTimeInGame;
    public int seasonCount;
    private bool _isWinter;
    
    public static TimeManager Instance;

    public float CurrentTimeOfDay
    {
        get => currentTimeOfDay;
        set => currentTimeOfDay = value;
    }

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
    
    private void Update()
    {
        // if(!Preset)
        //     return;
        if (Application.isPlaying)
        {
            CurrentTimeOfDay += Time.deltaTime;
            totalTimeInGame += Time.deltaTime;
            
            CurrentTimeOfDay %= 300f;
            LightingManager.Instance.UpdateLighting(CurrentTimeOfDay / 300f);
            LightingManager.Instance.ChangeSkybox();

            ControlChangeSeason();
        }
        else
        {
            LightingManager.Instance.UpdateLighting(CurrentTimeOfDay / 300f);
        }

    }

    private void ControlChangeSeason()
    {
        if (seasonTime - totalTimeInGame % seasonTime > 0.01f) return;
        
        seasonCount++;
        totalTimeInGame = seasonCount * seasonTime;
        if(seasonCount % 2 == 1)
        {
            StartCoroutine(TileManager.Instance.HandleWinter(true));
            _isWinter = true;
        }
        else
        {
            StartCoroutine(TileManager.Instance.HandleWinter(false));
            _isWinter = false;
        }
        
        Debug.Log("controlChangeSeason, season count = " + seasonCount + " totaltime = " + totalTimeInGame + " iswinter = " + _isWinter);
        
    }
}
