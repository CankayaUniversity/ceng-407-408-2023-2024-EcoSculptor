using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Settings menüsü açılır

            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }
}
