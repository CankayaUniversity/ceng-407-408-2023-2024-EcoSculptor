using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenToggle : MonoBehaviour
{
    private bool isFullScreen = true; // Start in fullscreen mode

    void Start()
    {
        // Initialize the screen mode
        Screen.fullScreen = isFullScreen;
    }

    public void ToggleFullScreenMode()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        Debug.Log("Fullscreen mode: " + isFullScreen);
    }
}
