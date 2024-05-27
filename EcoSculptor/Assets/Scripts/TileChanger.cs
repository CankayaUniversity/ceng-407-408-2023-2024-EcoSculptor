using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class TileChanger : SelectionManager
{
    private GameObject _tilePrefab;
    private GameObject _tileWinterPrefab;
    private TileClassifier _tileClassifier;
    private Tween newTween;
    private bool isSafeToClick;
    
    public GameObject TilePrefab
    {
        get => _tilePrefab;
        set => _tilePrefab = value;
    }

    public GameObject TileWinterPrefab
    {
        get => _tileWinterPrefab;
        set => _tileWinterPrefab = value;
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
        newTile.tag = _tilePrefab.tag;
        _selectedHex.tag = newTile.tag;
        if (_selectedHex.FoodFlag)
        {
            Destroy(_selectedHex.Food);
            _selectedHex.FoodFlag = false;
        }

        var winterHandler = newTile.GetComponentInChildren<WinterHandler>();
        var winterTilesDict = TileManager.Instance.WinterHandlersDict;

        if (winterHandler)
        {
            if (!winterTilesDict.TryAdd(oldTile.transform.position, winterHandler))
                winterTilesDict[oldTile.transform.position] = newTile.GetComponentInChildren<WinterHandler>();
        }
        else
        {
            if (winterTilesDict.ContainsKey(oldTile.transform.position))
                winterTilesDict.Remove(oldTile.transform.position);
        }

        
        var beginY = newTile.transform.position.y + 5;
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
        
        //AnimalSpawner.Instance.SpawnAnimals();
        _selectedHex.ControlRiver();
        
    }

    
    
}