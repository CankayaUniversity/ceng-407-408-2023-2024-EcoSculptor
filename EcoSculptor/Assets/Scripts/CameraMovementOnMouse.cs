using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CameraMovementOnMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 _cameraPosition;
    //private CinemachineTransposer _cinemachineTransposer;
    private bool _isOver;
    [Header("Camera Settings")] 
    [SerializeField] private float cameraSpeed = 10f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        
        //_cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (virtualCamera != null) _cameraPosition = virtualCamera.transform.position;
    }

    private void FixedUpdate()
    {
        if(_isOver)
            MoveCameraMouse();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isOver = true;
        virtualCamera.transform.position = _cameraPosition;

    }
    

    public void OnPointerExit(PointerEventData eventData)
    {
        _isOver = false;
        virtualCamera.transform.position = _cameraPosition;
    }
    
    private void MoveCameraMouse()
    {
        // if (Input.mouseScrollDelta.y > 0)
        // {
        //     _cinemachineTransposer.m_FollowOffset = new Vector3(0, 1, -10);
        // }
        switch (name)
        {
            case "Right":
                _cameraPosition.x += cameraSpeed * Time.fixedDeltaTime;
                break;
            
            case "Left":
                _cameraPosition.x -= cameraSpeed * Time.fixedDeltaTime;
                break;
            
            case "Top":
                _cameraPosition.z += cameraSpeed * Time.fixedDeltaTime;
                break;
            
            case "Down":
                _cameraPosition.z -= cameraSpeed * Time.fixedDeltaTime;
                break; 
        }
        virtualCamera.transform.position = _cameraPosition;
        Debug.Log(virtualCamera.transform.position);
    }
}
