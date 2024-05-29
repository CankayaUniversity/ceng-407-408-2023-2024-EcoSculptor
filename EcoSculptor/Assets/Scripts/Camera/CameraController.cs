using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float thresholdX = 960f;
    [SerializeField] private float thresholdY = 535f;
    
    [SerializeField] private float cameraMovementSpeed = 20f;
    [SerializeField] private float cameraRotateSpeed = 80f;
    [SerializeField] private float rightClickSensitivity = 800f;      //For right click rotation
    [SerializeField] private float cameraScrollSpeed = 100f;
    [SerializeField] private float cameraScrollRotateSpeed = 150f;
    
    [SerializeField] private float controlZUp = 45f;
    [SerializeField] private float controlZDown = -65f;
    [SerializeField] private float controlXLeft = -56f;
    [SerializeField] private float controlXRight = 51f;
    
    private int _width;
    private int _height;
    private Vector2 _mouseTurn;
    private bool _isSafe = true;
    
    private void Start()
    {
        _width = Screen.width;
        _height = Screen.height;

        thresholdX = thresholdX * _width / 1920;
        thresholdY = thresholdY * _height / 1080;
    }

    private void LateUpdate()
    {
        if (!transform) return;
        MoveCamera();
        RotateCamera();
        RotateCameraOnMouse();
        CameraZoomInOut();

    }

    private void MoveCamera()
    {
        var transform1 = transform;
        var forward = transform1.forward;
        var right = transform1.right;
        var mousePos = Input.mousePosition;
    
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        var position = transform.position;
    
        if (Input.GetKey(KeyCode.W) || mousePos.y >= (_height / 2f) + thresholdY)
        {
            position = Vector3.Lerp(position, position + forward, cameraMovementSpeed * Time.deltaTime);
            //transform.position +=  forward * (cameraSpeed * Time.deltaTime);
        }
    
        if (Input.GetKey(KeyCode.S) || mousePos.y <= (_height / 2f) - thresholdY)
        {
            position = Vector3.Lerp(position, position - forward, cameraMovementSpeed * Time.deltaTime);
            //transform.position -= forward * (cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A) || mousePos.x <= (_width / 2f) - thresholdX)
        {
            position = Vector3.Lerp(position, position - right, cameraMovementSpeed * Time.deltaTime);
            //transform.position -= right * (cameraSpeed * Time.deltaTime);}
        }

        if (Input.GetKey(KeyCode.D) || mousePos.x >= (_width / 2f) + thresholdX)
        {
            position = Vector3.Lerp(position, position + right, cameraMovementSpeed * Time.deltaTime);
            //transform.position += right * (cameraSpeed * Time.deltaTime);
        }
        
        position.z = Mathf.Clamp(position.z, controlZDown, controlZUp);
        position.x = Mathf.Clamp(position.x, controlXLeft, controlXRight);
        
        transform.position = position;
        
        // if (position.z > controlZUp)
        //     position.z = controlZUp;
        // if (position.z < controlZDown)
        //     position.z = controlZDown;
        // if (position.x > controlXRight)
        //     position.x = controlXRight;
        // if (position.x < controlXLeft)
        //     position.x = controlXLeft;
        // transform.position = position;
    }
    
    private void RotateCamera()
    {
        var pos = new Vector3(0, cameraRotateSpeed * Time.deltaTime, 0);
        var eulerAngles = transform.eulerAngles;
        
        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles = Vector3.Lerp(eulerAngles, eulerAngles - pos, cameraRotateSpeed * Time.deltaTime);
            //transform.eulerAngles -= new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles = Vector3.Lerp(eulerAngles, eulerAngles + pos, cameraRotateSpeed * Time.deltaTime);
            //transform.eulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        }
    }

    private void RotateCameraOnMouse()
    {
        if (!Input.GetMouseButton(1)) return;
        var rotation = transform.rotation;
        //var q = Quaternion.Euler(rotation.eulerAngles.x, _mouseTurn.x, rotation.eulerAngles.z);
        
        _mouseTurn.x = rotation.eulerAngles.y;
        _mouseTurn.x += Input.GetAxis("Mouse X") * rightClickSensitivity * Time.deltaTime;

        var v = new Vector3(rotation.eulerAngles.x, _mouseTurn.x, rotation.eulerAngles.z);
        transform.DORotate(v, 0.1f).SetEase(Ease.OutQuad);     // Sensitivity = 800
        
        //transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, _mouseTurn.x, rotation.eulerAngles.z);       Sensitivity = 300

    }

    private void CameraZoomInOut()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f && transform.position.y <= 1.0f ||
            Input.GetAxis("Mouse ScrollWheel") < 0.0f && transform.position.y >= 21.0f || 
            Input.GetAxis("Mouse ScrollWheel") == 0.0f) return;


        var transform1 = transform;
        var position = transform1.position;
        var eulerAngles = transform1.eulerAngles;

        var forward = transform1.forward;
        forward.y = 0f;
        
        var py = position.y - Input.GetAxis("Mouse ScrollWheel") * cameraScrollSpeed;
        var positionY = Mathf.Clamp(py, 1.0f, 21.0f);
        
        var newPos = position + forward * (Input.GetAxis("Mouse ScrollWheel") * cameraScrollSpeed);
        newPos.y = positionY;
        newPos.x = Mathf.Clamp(newPos.x, controlXLeft, controlXRight);
        newPos.z = Mathf.Clamp(newPos.z, controlZDown, controlZUp);

        var rX = eulerAngles.x - Input.GetAxis("Mouse ScrollWheel") * cameraScrollRotateSpeed;
        var rotateX = Mathf.Clamp(rX, 20.0f, 50.0f);
        
        var xRotationChange = new Vector3(rotateX, eulerAngles.y, eulerAngles.z);

        if(!_isSafe) return;
        
        _isSafe = false;
        transform.DOMove(newPos, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            _isSafe = true;
        });
        
        transform.DORotate(xRotationChange, 0.5f).SetEase(Ease.OutQuad);
        
    }
}