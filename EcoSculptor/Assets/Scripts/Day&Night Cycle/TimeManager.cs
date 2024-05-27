using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField, Range(0, 300f)] private float currentTimeOfDay = 100;
    [SerializeField, Range(0, 1000f)] private float seasonTime = 900f;
    
    private float _totalTimeInGame;
    private bool _isWinter;
    private int _seasonCount = 0;
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
            _totalTimeInGame = CurrentTimeOfDay;
            
            CurrentTimeOfDay %= 300f;
            LightingManager.Instance.UpdateLighting(CurrentTimeOfDay / 300f);
            LightingManager.Instance.ChangeSkybox();
        }
        else
        {
            LightingManager.Instance.UpdateLighting(CurrentTimeOfDay / 300f);
        }

    }

    private void ControlChangeSeason()
    {
        if (seasonTime - _totalTimeInGame % seasonTime <= 0.01f && seasonTime - _totalTimeInGame % seasonTime >= 0)
        {
            
        }
    }
}
