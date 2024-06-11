using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BackgroundMusic_Script : MonoBehaviour
{
    public static BackgroundMusic_Script Instance;
    private float volumeLevel = 0.5f;
    [SerializeField] private AudioSource myAudioSource;
    [SerializeField] private Slider volume;

    public AudioSource MyAudioSource
    {
        get => myAudioSource;
        set => myAudioSource = value;
    }

    public float VolumeLevel
    {
        get => volumeLevel;
        set => volumeLevel = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        volume = FindObjectOfType<Slider>();
        myAudioSource.volume = volumeLevel;

    }

    public void SetVoiceVal(float val)
    {
        volumeLevel = val;
        myAudioSource.volume = volumeLevel;
    }

}
