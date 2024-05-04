using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 10f;
    [SerializeField] private float rotateSpeed = 40f;
    [SerializeField] private float thresholdX = 960f;
    [SerializeField] private float thresholdY = 535f;
    [SerializeField] private float sensitivity = 200f; 
    [SerializeField] private float controlZUp = 30f;
    [SerializeField] private float controlZDown = -70f;
    [SerializeField] private float controlXLeft = -50f;
    [SerializeField] private float controlXRight = 50f;
    private int _width;
    private int _height;
    private Vector2 _mouseTurn;
    private Camera _mainCamera;

    private void Start()
    {
        _width = Screen.width;
        _height = Screen.height;

        thresholdX = thresholdX * _width / 1920;
        thresholdY = thresholdY * _height / 1080;
        _mainCamera = Camera.main;

    }
    
    private void LateUpdate()
    {
        if(!transform) return;
        MoveCamera();
        RotateCamera();
        RotateCameraOnMouse();
    }
    
    private void MoveCamera()
    {
        var camTransform = transform;
        var forward = camTransform.forward;
        var right = camTransform.right;
        var mousePos = Input.mousePosition;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
    
        if ((Input.GetKey(KeyCode.W) || mousePos.y >= (_height / 2f) + thresholdY) && transform.position.z < controlZUp) { transform.position +=  forward * (cameraSpeed * Time.deltaTime); }
        
        if ((Input.GetKey(KeyCode.S) || mousePos.y <= (_height / 2f) - thresholdY) && transform.position.z > controlZDown) { transform.position -= forward * (cameraSpeed * Time.deltaTime); }
        
        if ((Input.GetKey(KeyCode.A) || mousePos.x <= (_width / 2f) - thresholdX) && transform.position.x > controlXLeft) { transform.position -= right * (cameraSpeed * Time.deltaTime); }
        
        if ((Input.GetKey(KeyCode.D) || mousePos.x >= (_width / 2f) + thresholdX) && transform.position.x < controlXRight) { transform.position += right * (cameraSpeed * Time.deltaTime);} 
    }
    
    private void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Q)) { transform.eulerAngles -= new Vector3(0, rotateSpeed * Time.deltaTime, 0); }
        
        if (Input.GetKey(KeyCode.E)) { transform.eulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime, 0); }

        // if (transform.eulerAngles.y is <= 360 and >= 0) return;
        // var camTransform = transform;
        // camTransform.eulerAngles = new Vector3(0, camTransform.eulerAngles.y % 360, 0);

    }

    private void RotateCameraOnMouse()
    {
        if (!Input.GetMouseButton(1)) return;
        var rotation = transform.rotation;
        _mouseTurn.x = rotation.eulerAngles.y;
        _mouseTurn.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, _mouseTurn.x, rotation.eulerAngles.z);

    }
}