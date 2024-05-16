using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class TileChanger : SelectionManager
{
    [SerializeField] private GameObject defaultTile;
    
    private GameObject _tilePrefab;
    private TileClassifier _tileClassifier;
    private Tween newTween;
    private bool isSafeToClick;
    
    public GameObject TilePrefab
    {
        get => _tilePrefab;
        set => _tilePrefab = value;
    }

    private void Start()
    {
        _tileClassifier = GetComponent<TileClassifier>();
        isSafeToClick = true;
    }

    public override void GetHexAndOutline(Vector3 mousePosition)
    {
        base.GetHexAndOutline(mousePosition);
        ChangeTile();
    }

    private void ChangeTile()
    {
        if(!_tilePrefab || !isSafeToClick) return;
        
        var oldTile = SelectedHex.TileMesh;
        
        Destroy(SelectedHex.TileMesh);

        var newTile = SelectedHex.TileMesh = Instantiate(_tilePrefab, SelectedHex.TileMeshParent);

        var beginY = newTile.transform.position.y - 5;
        var endY = newTile.transform.position.y;

        isSafeToClick = false;
        newTween?.Kill();
        newTween = DOVirtual.Float(beginY, endY, .5f, v =>
        {
            var position = newTile.transform.position;
            position.y = v;
            newTile.transform.position = position;
        }).SetEase(Ease.OutBack).OnComplete(() => isSafeToClick = true);
        
        TileManager.Instance.TileCountOnChangeHandler(newTile.tag, oldTile.tag);
    }


    public void ChangeTileToDefault(Vector3 mousePos)
    {
        base.GetHexAndOutline(mousePos);
        
        var oldTile = SelectedHex.TileMesh;
        Destroy(SelectedHex.TileMesh);
        var newTile = SelectedHex.TileMesh = Instantiate(defaultTile, SelectedHex.TileMeshParent);
        
        TileManager.Instance.TileCountOnChangeHandler(newTile.tag, oldTile.tag);
    }

    
    
}