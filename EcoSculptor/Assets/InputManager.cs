using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Reference to the settings GameObject
    public GameObject settingsMenu;
    private bool isActive;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        settingsMenu.SetActive(false);
    }
    
    
    public void StopGame()
    {
        Time.timeScale = 0;
        settingsMenu.SetActive(true);
    }
}