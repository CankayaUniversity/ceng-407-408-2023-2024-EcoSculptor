using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("go to play game " + VolumeSlider.Instance.volumeSlider.value);
        BackgroundMusic_Script.Instance.SetVoiceVal(VolumeSlider.Instance.volumeSlider.value);
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("go to main menu " + VolumeSlider.Instance.volumeSlider.value);
        BackgroundMusic_Script.Instance.SetVoiceVal(VolumeSlider.Instance.volumeSlider.value);
        //Time.timeScale = 1;
        //InputManager.Instance.StopGame();
        //slider.volumeSlider.value = BackgroundMusic_Script.Instance.MyAudioSource.volume;
        
    }
    
    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
}
