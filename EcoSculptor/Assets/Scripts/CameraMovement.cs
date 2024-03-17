using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 10f;
    [SerializeField] private float rotateSpeed = 40f;
    [SerializeField] private float thresholdX = 960f;
    [SerializeField] private float thresholdY = 535f;
    private int _width;
    private int _height;
    private bool _rightClicked;
    private float _xRotation;
    private Vector3 _cameraPosition;
    private Vector3 _oldMousePosition;

    private void Start()
    {
        _width = Screen.width;
        _height = Screen.height;

        thresholdX = thresholdX * _width / 1920;
        thresholdY = thresholdY * _height / 1080;
        _cameraPosition = transform.position;
    }
    
    private void LateUpdate()
    {
        MoveCameraOnMouse();
        RotateCameraOnMouse();
        MoveCamera();
        RotateCamera();
    }
    
    private void RotateCamera()
    {
        if(!transform) return;
        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles -= new Vector3(0, rotateSpeed * Time.deltaTime, 0);

        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime, 0);

        }
    }
    
    private void MoveCamera()
    {
        if(!transform) return;
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

    private void MoveCameraOnMouse()
    {
        if(!transform) return;
        var mousePos = Input.mousePosition;
        if (mousePos.x >= (_width / 2f) + thresholdX)
        {
            _cameraPosition.x += cameraSpeed * Time.deltaTime;
        }
        if (mousePos.x <= (_width / 2f) - thresholdX)
        {
            _cameraPosition.x -= cameraSpeed * Time.deltaTime;
        }
        if (mousePos.y >= (_height / 2f) + thresholdY)
        {
            _cameraPosition.z += cameraSpeed * Time.deltaTime;
        }
        if(mousePos.y <= (_height / 2f) - thresholdY)
        {
            _cameraPosition.z -= cameraSpeed * Time.deltaTime;
        }
        transform.position = _cameraPosition;
    }

    private void RotateCameraOnMouse()
    {
        if( Input.GetMouseButtonDown(1))
        {
            _oldMousePosition = Input.mousePosition;
            return;
        }


        if (!Input.GetMouseButton(1)) return;
        var currentMousePosition = Input.mousePosition;                
                               
        if ( currentMousePosition.x < _oldMousePosition.x)
        {
            transform.eulerAngles -= new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        }

        if (currentMousePosition.x > _oldMousePosition.x)
        {
            transform.eulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        }
    }
}