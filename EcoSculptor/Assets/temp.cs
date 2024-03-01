using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[SelectionBase]
public class temp : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private Transform child;
    

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(child.GetChild(0).gameObject);

            Instantiate(tile, child);
        }
    }
}
