﻿
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WinterHandler : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject winterTile;

    private Tween newTween;

    private void Start()
    {
        //TileManager.Instance.RegisterToList(this);
    }

    private void WinterMode()
    {
        winterTile.SetActive(true);
        tile.SetActive(false);
    }

    private void NormalMode()
    {
        winterTile.SetActive(false);
        tile.SetActive(true);
    }

    public void ChangeTileToWinter()
    {
        winterTile.SetActive(true);
        tile.SetActive(false);

        var position1 = winterTile.transform.position;
        var beginY = position1.y + 8;
        var endY = position1.y;
        
        newTween?.Kill();
        newTween = DOVirtual.Float(beginY, endY, .5f, v =>
        {
            var position = winterTile.transform.position;
            position.y = v;
            winterTile.transform.position = position;
        }).SetEase(Ease.OutBack);
    }
    
    public void ChangeTileToNormal()
    {
        winterTile.SetActive(false);
        tile.SetActive(true);

        var position1 = tile.transform.position;
        var beginY = position1.y + 8;
        var endY = position1.y;
        
        newTween?.Kill();
        newTween = DOVirtual.Float(beginY, endY, .5f, v =>
        {
            var position = tile.transform.position;
            position.y = v;
            tile.transform.position = position;
        }).SetEase(Ease.OutBack);
    }
}