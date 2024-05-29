using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public static VolumeSlider Instance;

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

    public void ChangeVolume()
    {
        BackgroundMusic_Script.Instance.MyAudioSource.volume = volumeSlider.value;
    }

    private void Start()
    {
        Debug.Log("start");
        volumeSlider.value = FindObjectOfType<BackgroundMusic_Script>().VolumeLevel;
    }
}
