using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{

    private void Start()
    {
    }

    private void LateUpdate()
    {
        transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

    }
}
