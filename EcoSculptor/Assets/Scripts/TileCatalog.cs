using System;
using UnityEngine;

[RequireComponent(typeof(TileChanger))]
public class TileCatalog : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject river;
    [SerializeField] private GameObject grass;
    [SerializeField] private GameObject desert;
    [SerializeField] private GameObject stone;
    
    private TileChanger _tileChanger;

    private void Start()
    {
        _tileChanger = GetComponent<TileChanger>();
    }

    public void WaterTile()
    {
        _tileChanger.TilePrefab = water;
    }

    public void RiverTile()
    {
        _tileChanger.TilePrefab = river;
    }
}