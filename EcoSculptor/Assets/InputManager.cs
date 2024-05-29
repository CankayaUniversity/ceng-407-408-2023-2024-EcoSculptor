using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    // Reference to the settings GameObject
    public GameObject settingsMenu;
    private bool isActive;
    public static InputManager Instance;
    
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
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            // Toggle the settings menu visibility
            if (!settingsMenu) return;
            
            isActive = settingsMenu.activeSelf;
            
            if (isActive)
                ResumeGame();
            else
                StopGame();
            
        }
    }

    public void  ResumeGame()
    {
        Time.timeScale = 1;
        if (!settingsMenu) return;
        settingsMenu.SetActive(false);
    }
    
    
    public void StopGame()
    {
        Time.timeScale = 0;
        if (!settingsMenu) return;
        settingsMenu.SetActive(true);
    }
}