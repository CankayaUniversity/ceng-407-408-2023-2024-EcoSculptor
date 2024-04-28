using System;
using UnityEngine;
using UnityEngine.UIElements;

public class TileChanger : SelectionManager
{
    private GameObject _tilePrefab;
    private TileClassifier _tileClassifier;
    
    public GameObject TilePrefab
    {
        get => _tilePrefab;
        set => _tilePrefab = value;
    }

    private void Start()
    {
        _tileClassifier = GetComponent<TileClassifier>();
    }

    public override void GetHexAndOutline(Vector3 mousePosition)
    {
        base.GetHexAndOutline(mousePosition);
        ChangeTile();
    }

    private void ChangeTile()
    {
        if(!_tilePrefab) return;
        
        var oldTile = SelectedHex.TileMesh;
        Destroy(SelectedHex.TileMesh);
        var newTile = SelectedHex.TileMesh = Instantiate(_tilePrefab, SelectedHex.TileMeshPrent);
        
        TileManager.Instance.TileCountOnChangeHandler(newTile.tag, oldTile.tag);
    }

    
    
}