using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _cameraPosition;

    [Header("Camera Settings")] 
    public float cameraSpeed = 10f;
    
    
    
    void Start()
    {
        _cameraPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _cameraPosition.z += cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _cameraPosition.z -= cameraSpeed * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.A))
        {
            _cameraPosition.x -= cameraSpeed * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.D))
        {
            _cameraPosition.x += cameraSpeed * Time.deltaTime;

        }

        transform.position = _cameraPosition;


    }
}
