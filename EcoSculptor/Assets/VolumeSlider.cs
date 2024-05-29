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
        AudioListener.volume = volumeSlider.value;
    }

    private void Start()
    {
        volumeSlider.value = FindObjectOfType<BackgroundMusic_Script>().VolumeLevel;
    }
}
