using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    private Vector3 _minimapPosition;
    //private Vector3 _minimapEulerAngles;

    private void Start()
    {
        _minimapPosition = transform.position;
        //_minimapEulerAngles = transform.eulerAngles;
    }

    void LateUpdate()
    {
        _minimapPosition.x = Camera.main.transform.position.x;
        _minimapPosition.z = Camera.main.transform.position.z;

        //_minimapEulerAngles.y = Camera.main.transform.eulerAngles.y;
        

        transform.position = _minimapPosition;
        //transform.eulerAngles = _minimapEulerAngles;

    }
}
