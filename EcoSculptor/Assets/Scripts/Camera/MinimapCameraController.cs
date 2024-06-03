using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 euler;
    void Start()
    {
        pos = Camera.main.transform.position;
        euler = transform.eulerAngles;
    }

    void LateUpdate()
    {
        pos.x = Camera.main.transform.position.x;
        pos.z = Camera.main.transform.position.z;

        euler.y = Camera.main.transform.eulerAngles.y;

        transform.eulerAngles = euler;
        transform.position = pos;
    }
}
