using System;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [Header("Component Elements")] 
    [SerializeField] private Outline outline;
    [SerializeField] private Transform tileMeshParent;
    [SerializeField] private GameObject tileMesh;
    
    
    private HexCoordinates _hexCoordinates;
    
    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    public Outline Outline
    {
        get => outline;
        set => outline = value;
    }


    public Transform TileMeshParent
    {
        get => tileMeshParent;
        set => tileMeshParent = value;
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

    private void OnEnable()
    {
        TileManager.Instance.RegisterTile(tileMesh.gameObject.tag);
    }
}
