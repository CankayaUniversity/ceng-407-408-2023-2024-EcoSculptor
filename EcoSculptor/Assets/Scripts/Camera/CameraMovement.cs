using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 20f;
    [SerializeField] private float rotateSpeed = 80f;
    [SerializeField] private float thresholdX = 960f;
    [SerializeField] private float thresholdY = 535f;
    [SerializeField] private float sensitivity = 800f; 
    [SerializeField] private float controlZUp = 45f;
    [SerializeField] private float controlZDown = -65f;
    [SerializeField] private float controlXLeft = -56f;
    [SerializeField] private float controlXRight = 51f;
    [SerializeField] private float scrollSpeed = 15000f;
    [SerializeField] private float scrollRotateSpeed = 1000f;
    
    private int _width;
    private int _height;
    private Vector2 _mouseTurn;
    private bool _isSafe = true;
    private Camera _mainCamera;
    private Vector3 initialPosition;
    private Vector3 maxForward;
    private Vector3 minForward;
    private Vector3 maxRight;
    private Vector3 minRight;
    
    private void Start()
    {
        _width = Screen.width;
        _height = Screen.height;

        thresholdX = thresholdX * _width / 1920;
        thresholdY = thresholdY * _height / 1080;
        _mainCamera = Camera.main;
        
        initialPosition = transform.position;
    }
    
    private void LateUpdate()
    {
        if(!transform) return;
        MoveCamera();
        RotateCamera();
        RotateCameraOnMouse();
        CameraZoomInOut();

    }
    // private void MoveCamera()
    // {
    //     var forward = transform.forward;
    //     var right = transform.right;
    //     var mousePos = Input.mousePosition;
    //
    //     forward.y = 0f;
    //     right.y = 0f;
    //     forward.Normalize();
    //     right.Normalize();
    //     var position = transform.position;
    //     
    //     if (Input.GetKey(KeyCode.W) || mousePos.y >= _height / 2f + thresholdY)
    //     {
    //         position = Vector3.Lerp(position, position + forward, cameraSpeed * Time.deltaTime);
    //         //transform.position +=  forward * (cameraSpeed * Time.deltaTime);
    //     }
    //
    //     if (Input.GetKey(KeyCode.S) || mousePos.y <= _height / 2f - thresholdY)
    //     {
    //         position = Vector3.Lerp(position, position - forward, cameraSpeed * Time.deltaTime);
    //         //transform.position -= forward * (cameraSpeed * Time.deltaTime);
    //     }
    //
    //     if (Input.GetKey(KeyCode.A) || mousePos.x <= _width / 2f - thresholdX)
    //     {
    //         position = Vector3.Lerp(position, position - right, cameraSpeed * Time.deltaTime);
    //         //transform.position -= right * (cameraSpeed * Time.deltaTime);
    //     }
    //
    //     if (Input.GetKey(KeyCode.D) || mousePos.x >= _width / 2f + thresholdX)
    //     {
    //         position = Vector3.Lerp(position, position + right, cameraSpeed * Time.deltaTime);
    //         //transform.position += right * (cameraSpeed * Time.deltaTime);
    //     } 
    //     // Calculate the constrained position
    //     //position = ConstrainPosition(position);
    //
    //     // Apply the constrained position to the camera
    //     transform.position = position;
    //
    // }
    
    // private Vector3 ConstrainPosition(Vector3 position)
    // {
    //     var rotatedInitialPosition = Quaternion.Euler(0, transform.eulerAngles.y, 0) * initialPosition;
    //
    //     if (transform.eulerAngles.y >= -45 && transform.eulerAngles.y <= 45)
    //     {
    //         maxForward = rotatedInitialPosition + transform.forward * controlZUp;
    //         minForward = rotatedInitialPosition + transform.forward * controlZDown;
    //         maxRight = rotatedInitialPosition + transform.right * controlXRight;
    //         minRight = rotatedInitialPosition + transform.right * controlXLeft;
    //     }
    //     else if (transform.eulerAngles.y > 45 && transform.eulerAngles.y < 135)
    //     {
    //         maxForward = rotatedInitialPosition + transform.right * controlZUp;
    //         minForward = rotatedInitialPosition + transform.right * controlZDown;
    //         maxRight = rotatedInitialPosition + transform.forward * controlXRight;
    //         minRight = rotatedInitialPosition + transform.forward * controlXLeft;
    //
    //     }
    //     else if (transform.eulerAngles.y >= 135 && transform.eulerAngles.y <= -135)
    //     {
    //         maxForward = rotatedInitialPosition + transform.forward * controlZUp;
    //         minForward = rotatedInitialPosition + transform.forward * controlZDown;
    //         maxRight = rotatedInitialPosition + transform.right * controlXRight;
    //         minRight = rotatedInitialPosition + transform.right * controlXLeft;
    //     }
    //     else
    //     {
    //         Debug.Log("VAR");
    //         maxForward = rotatedInitialPosition + transform.right * controlZUp;
    //         minForward = rotatedInitialPosition + transform.right * controlZDown;
    //         maxRight = rotatedInitialPosition + transform.forward * controlXRight;
    //         minRight = rotatedInitialPosition + transform.forward * controlXLeft;
    //     }
    //
    //     // Clamp the position based on these bounds
    //     var minX = Mathf.Min(minRight.x, maxRight.x);
    //     var maxX = Mathf.Max(minRight.x, maxRight.x);
    //     var minZ = Mathf.Min(minForward.z, maxForward.z);
    //     var maxZ = Mathf.Max(minForward.z, maxForward.z);
    //
    //     position.x = Mathf.Clamp(position.x, minX, maxX);
    //     position.z = Mathf.Clamp(position.z, minZ, maxZ);
    //
    //     return position;
    // }

    private void MoveCamera()
    {
        var forward = transform.forward;
        var right = transform.right;
        var mousePos = Input.mousePosition;
    
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        var position = transform.position;
    
        if (Input.GetKey(KeyCode.W) || mousePos.y >= (_height / 2f) + thresholdY)
        {
            position = Vector3.Lerp(position, position + forward, cameraSpeed * Time.deltaTime);
            //transform.position +=  forward * (cameraSpeed * Time.deltaTime);
        }
    
        if (Input.GetKey(KeyCode.S) || mousePos.y <= (_height / 2f) - thresholdY)
        {
            position = Vector3.Lerp(position, position - forward, cameraSpeed * Time.deltaTime);
            //transform.position -= forward * (cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A) || mousePos.x <= (_width / 2f) - thresholdX)
        {
            position = Vector3.Lerp(position, position - right, cameraSpeed * Time.deltaTime);
            //transform.position -= right * (cameraSpeed * Time.deltaTime);}
        }

        if (Input.GetKey(KeyCode.D) || mousePos.x >= (_width / 2f) + thresholdX)
        {
            position = Vector3.Lerp(position, position + right, cameraSpeed * Time.deltaTime);
            //transform.position += right * (cameraSpeed * Time.deltaTime);
        }

        if (position.z > controlZUp)
            position.z = controlZUp;
        if (position.z < controlZDown)
            position.z = controlZDown;
        if (position.x > controlXRight)
            position.x = controlXRight;
        if (position.x < controlXLeft)
            position.x = controlXLeft;
        transform.position = position;
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

        var v = new Vector3(rotation.eulerAngles.x, _mouseTurn.x, rotation.eulerAngles.z);
        transform.DORotate(v, 0.1f).SetEase(Ease.OutQuad);     // Sensitivity = 800
        
        //transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, _mouseTurn.x, rotation.eulerAngles.z);       Sensitivity = 300

    }

    private void CameraZoomInOut()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f && transform.position.y <= 2.0f ||
            Input.GetAxis("Mouse ScrollWheel") < 0.0f && transform.position.y >= 30.0f ) return;

        var trans = transform;
        var pos = trans.position;
        var eulerAngles = trans.eulerAngles;

        var forward = trans.forward;
        forward.y = 0f;
        
        var py = pos.y - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        var positionY = Mathf.Clamp(py, 1.0f, 30.0f);
        
        var newPos = pos + forward * (Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime);
        newPos.y = positionY;
        newPos.x = Mathf.Clamp(newPos.x, controlXLeft, controlXRight);
        newPos.z = Mathf.Clamp(newPos.z, controlZDown, controlZUp);
        
        
        var rXIn = eulerAngles.x - scrollRotateSpeed * Time.deltaTime;
        var rXOut = eulerAngles.x + scrollRotateSpeed * Time.deltaTime;
        var rotateXIn = Mathf.Clamp(rXIn, 0.0f, 50.0f);
        var rotateXOut = Mathf.Clamp(rXOut, 0.0f, 50.0f);
        
        var xChangedZoomIn = new Vector3(rotateXIn, eulerAngles.y, eulerAngles.z);
        var xChangedZoomOut = new Vector3(rotateXOut, eulerAngles.y, eulerAngles.z);


        if (!(Input.GetAxis("Mouse ScrollWheel") > 0.0f) && !(Input.GetAxis("Mouse ScrollWheel") < 0.0f)) return;
        if(!_isSafe) return;
        
        _isSafe = false;
        transform.DOMove(newPos, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            _isSafe = true;
        });
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            transform.DORotate(xChangedZoomIn, 0.5f).SetEase(Ease.OutQuad);

        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            transform.DORotate(xChangedZoomOut, 0.5f).SetEase(Ease.OutQuad);


    }
}