using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{

    private Vector3 pos;
    void Start()
    {
        pos = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        pos.x = Camera.main.transform.position.x;
        pos.z = Camera.main.transform.position.z;

        transform.position = pos;
    }
}
