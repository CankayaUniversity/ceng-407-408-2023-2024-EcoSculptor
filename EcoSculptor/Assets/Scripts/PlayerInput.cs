using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector3> PointerHover;
    public UnityEvent<Vector3> PointerClick;
    public UnityEvent<Vector3> pointerRightClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 mousePos = Input.mousePosition;
            PointerClick?.Invoke(mousePos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            
            Vector3 mousePos = Input.mousePosition;
            pointerRightClick?.Invoke(mousePos);
        }
    }

    private void LateUpdate()
    {
        HexOutline();
    }
    
    private void HexOutline()
    {
        Vector3 mousePos = Input.mousePosition;
        PointerHover?.Invoke(mousePos);
    }
}