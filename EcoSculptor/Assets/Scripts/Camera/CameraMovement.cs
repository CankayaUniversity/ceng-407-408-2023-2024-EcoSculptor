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
    [SerializeField] private float scrollSpeed = 500f;
    [SerializeField] private float scrollRotateSpeed = 20f;
    
    private int _width;
    private int _height;
    private Vector2 _mouseTurn;
    private bool _control;
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
        CameraZoomInOut();

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
        var position = transform.position;

        if ((Input.GetKey(KeyCode.W) || mousePos.y >= (_height / 2f) + thresholdY) && transform.position.z < controlZUp)
        {
            transform.position = Vector3.Lerp(position, position + forward, cameraSpeed * Time.deltaTime);
            //transform.position +=  forward * (cameraSpeed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.S) || mousePos.y <= (_height / 2f) - thresholdY) &&
            transform.position.z > controlZDown)
        {
            transform.position = Vector3.Lerp(position, position - forward, cameraSpeed * Time.deltaTime);
            //transform.position -= forward * (cameraSpeed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.A) || mousePos.x <= (_width / 2f) - thresholdX) &&
            transform.position.x > controlXLeft)
        {
            transform.position = Vector3.Lerp(position, position - right, cameraSpeed * Time.deltaTime);
            //transform.position -= right * (cameraSpeed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.D) || mousePos.x >= (_width / 2f) + thresholdX) &&
            transform.position.x < controlXRight)
        {
            transform.position = Vector3.Lerp(position, position + right, cameraSpeed * Time.deltaTime);
            //transform.position += right * (cameraSpeed * Time.deltaTime);
        } 
    }
    
    private void RotateCamera()
    {
        var pos = new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        var eulerAngles = transform.eulerAngles;
        
        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles = Vector3.Lerp(eulerAngles, eulerAngles - pos, rotateSpeed * Time.deltaTime);
            //transform.eulerAngles -= new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles = Vector3.Lerp(eulerAngles, eulerAngles + pos, rotateSpeed * Time.deltaTime);
            //transform.eulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        }
    }

    private void RotateCameraOnMouse()
    {
        if (!Input.GetMouseButton(1)) return;
        var rotation = transform.rotation;
        //var q = Quaternion.Euler(rotation.eulerAngles.x, _mouseTurn.x, rotation.eulerAngles.z);
        
        _mouseTurn.x = rotation.eulerAngles.y;
        _mouseTurn.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, _mouseTurn.x, rotation.eulerAngles.z);

        //transform.rotation = Quaternion.Lerp(rotation, q, sensitivity * Time.deltaTime);

    }

    private void CameraZoomInOut()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f && transform.position.y <= 2.0f ||
            Input.GetAxis("Mouse ScrollWheel") < 0.0f && transform.position.y >= 30.0f ) return;
        
        var pos = transform.position;
        var eulerAngles = transform.eulerAngles;
        var xChangedZoomIn = new Vector3(eulerAngles.x - scrollRotateSpeed * Time.deltaTime, eulerAngles.y, eulerAngles.z);
        var xChangedZoomOut = new Vector3(eulerAngles.x + scrollRotateSpeed * Time.deltaTime, eulerAngles.y, eulerAngles.z);
        
        var vec = new Vector3(pos.x, pos.y - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime,
            pos.z + Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime);
        
        transform.position = Vector3.Lerp(pos, vec, scrollSpeed * Time.deltaTime);
        
        if(transform.position.y < 4.0f && !_control)
        {
            transform.eulerAngles = Vector3.Lerp(eulerAngles, xChangedZoomIn, scrollRotateSpeed * Time.deltaTime);
            if(transform.eulerAngles.x <= 25.0f)
                _control = true;
        }

        if (transform.position.y > 4.0f && _control)
        {
            transform.eulerAngles = Vector3.Lerp(eulerAngles, xChangedZoomOut, scrollRotateSpeed * Time.deltaTime);
            if (transform.eulerAngles.x >= 50.0f)
                _control = false;

        }

    }
}