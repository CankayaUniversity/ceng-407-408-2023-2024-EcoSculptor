using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private BackgroundMusic_Script BGM;
    [SerializeField] private VolumeSlider slider;
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("go to play game " +slider.volumeSlider.value);
        BGM.SetVoiceVal(slider.volumeSlider.value);
        //Time.timeScale = 1;
        InputManager.Instance.ResumeGame();
        slider.volumeSlider.value = BackgroundMusic_Script.Instance.MyAudioSource.volume;
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("go to main menu " +slider.volumeSlider.value);
        BGM.SetVoiceVal(slider.volumeSlider.value);
        //Time.timeScale = 1;
        InputManager.Instance.StopGame();
        slider.volumeSlider.value = BackgroundMusic_Script.Instance.MyAudioSource.volume;
        
    }
    
    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void Start()
    {
        BGM = FindObjectOfType<BackgroundMusic_Script>();
    }
}
