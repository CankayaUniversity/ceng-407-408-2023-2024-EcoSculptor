using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector3> PointerHover;
    public UnityEvent<Vector3> PointerClick;

    public static PlayerInput Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            PointerClick?.Invoke(mousePos);
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