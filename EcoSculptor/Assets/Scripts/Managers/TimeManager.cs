using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeManager : MonoBehaviour
{
    [SerializeField, Range(0, 300f)] private float currentTimeOfDay = 100;
    [SerializeField, Range(0, 1000f)] private float seasonTime = 900f;
    //[SerializeField, Range(0, 1000f)] private float LoseControlTime = 900f;
    
    
    public float totalTimeInGame;
    public int seasonCount;
    private bool _isWinter;
    private Coroutine newRoutine;
    public static TimeManager Instance;

    public float CurrentTimeOfDay
    {
        get => currentTimeOfDay;
        set => currentTimeOfDay = value;
    }

    public Coroutine NewRoutine
    {
        get => newRoutine;
        set => newRoutine = value;
    }

    public bool IsWinter => _isWinter;

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

    private void OnDisable()
    {
        if (NewRoutine == null)return;
        StopCoroutine(nameof(LoseControl));
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _isWinter = true;
        if (Input.GetKeyDown(KeyCode.L))
            _isWinter = false;
        
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
        
        
    }

    public IEnumerator LoseControl()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            var totalAnimalCount = AnimalManager.Instance.TotalAnimalCount();
            if (totalAnimalCount == 0)
            {
                Debug.Log("game over");
                //game over scene will open
                break;
            }
        }
    }
}
