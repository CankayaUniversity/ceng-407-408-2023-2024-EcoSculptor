using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    private HexCoordinates _hexCoordinates;

    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
    }
}
