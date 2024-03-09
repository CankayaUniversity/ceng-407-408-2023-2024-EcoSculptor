using System;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [Header("Component Elements")]
    [SerializeField] private Outline outline;
    [SerializeField] private Transform tileMeshPrent;
    [SerializeField] private GameObject tileMesh;
    
    private HexCoordinates _hexCoordinates;
    
    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    public Outline Outline
    {
        get => outline;
        set => outline = value;
    }


    public Transform TileMeshPrent
    {
        get => tileMeshPrent;
        set => tileMeshPrent = value;
    }

    public GameObject TileMesh
    {
        get => tileMesh;
        set => tileMesh = value;
    }


    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
    }
}
