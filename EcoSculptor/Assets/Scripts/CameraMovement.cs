using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [Header("Camera Settings")] 
    [SerializeField] private float cameraSpeed = 10f;
    [SerializeField] private float rotateSpeed = 100f;
    
    private Vector3 _cameraPosition;

    
    private void Start()
    {
        _cameraPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MoveCamera();
        RotateCamera();
    }
    private void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles -= new Vector3(0, rotateSpeed * Time.fixedDeltaTime, 0);

        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles += new Vector3(0, rotateSpeed * Time.fixedDeltaTime, 0);

        }
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _cameraPosition.z += cameraSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _cameraPosition.z -= cameraSpeed * Time.fixedDeltaTime;

        }
        if (Input.GetKey(KeyCode.A))
        {
            _cameraPosition.x -= cameraSpeed * Time.fixedDeltaTime;

        }
        if (Input.GetKey(KeyCode.D))
        {
            _cameraPosition.x += cameraSpeed * Time.fixedDeltaTime;

        }

        transform.position = _cameraPosition;
    }

    public void ChangeCameraPosition(Vector3 cameraPos)
    {
        transform.position = cameraPos;

    }
    

    
}
