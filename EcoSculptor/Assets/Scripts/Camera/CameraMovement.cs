using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 20f;
    [SerializeField] private float rotateSpeed = 60f;
    [SerializeField] private float thresholdX = 960f;
    [SerializeField] private float thresholdY = 535f;
    [SerializeField] private float sensitivity = 300f; 
    [SerializeField] private float controlZUp = 30f;
    [SerializeField] private float controlZDown = -70f;
    [SerializeField] private float controlXLeft = -50f;
    [SerializeField] private float controlXRight = 50f;
    [SerializeField] private float scrollSpeed = 10000f;
    [SerializeField] private float scrollRotateSpeed = 1000f;
    
    private int _width;
    private int _height;
    private Vector2 _mouseTurn;
    private bool _isSafe = true;
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

        var trans = transform;
        var pos = trans.position;
        var eulerAngles = trans.eulerAngles;

        var forward = trans.forward;
        forward.y = 0f;
        
        var py = pos.y - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        var positionY = Mathf.Clamp(py, 1.0f, 30.0f);
        
        var newPos = pos + forward * (Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime);
        newPos.y = positionY;
        newPos.x = Mathf.Clamp(newPos.x, -50, 50);
        newPos.z = Mathf.Clamp(newPos.z, -65, 50);
        
        
        var rXIn = eulerAngles.x - scrollRotateSpeed * Time.deltaTime;
        var rXOut = eulerAngles.x + scrollRotateSpeed * Time.deltaTime;
        var rotateXIn = Mathf.Clamp(rXIn, 0.0f, 50.0f);
        var rotateXOut = Mathf.Clamp(rXOut, 0.0f, 50.0f);
        
        var xChangedZoomIn = new Vector3(rotateXIn, eulerAngles.y, eulerAngles.z);
        var xChangedZoomOut = new Vector3(rotateXOut, eulerAngles.y, eulerAngles.z);


        if (!(Input.GetAxis("Mouse ScrollWheel") > 0.0f) && !(Input.GetAxis("Mouse ScrollWheel") < 0.0f)) return;
        if(!_isSafe) return;
        
        _isSafe = false;
        transform.DOMove(newPos, 1.0f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            _isSafe = true;
        });
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            transform.DORotate(xChangedZoomIn, 1.0f).SetEase(Ease.OutQuad);

        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            transform.DORotate(xChangedZoomOut, 1.0f).SetEase(Ease.OutQuad);


    }
}