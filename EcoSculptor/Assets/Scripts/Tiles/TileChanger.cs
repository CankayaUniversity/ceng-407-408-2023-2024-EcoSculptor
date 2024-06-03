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
        
        var tilePrice = EconomyManager.Instance.TilePriceCatalog.GetTilePrice(_selectedHex.tag);
        var playerPrice = EconomyManager.Instance.ElementalResource;
        
        if(playerPrice < tilePrice) return;
        
        EconomyManager.Instance.DecreaseResource(tilePrice);
        
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

        SelectedHex.winterHandler = newTile.GetComponent<WinterHandler>();
        
        if(_selectedHex.winterHandler)
            if(TimeManager.Instance.IsWinter)
                _selectedHex.winterHandler.PutTileAsWinter();
        
        PutTile(newTile);

        TileManager.Instance.TileCountOnChangeHandler(newTile.tag, oldTile.tag);
        AnimalManager.Instance.SpawnAnimals();
        _selectedHex.ControlRiver();
    }

    private void PutTile(GameObject newTile)
    {
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
    }
}