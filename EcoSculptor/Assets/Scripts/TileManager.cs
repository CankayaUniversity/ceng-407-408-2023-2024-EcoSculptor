using System;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private int _grassTile;
    [SerializeField] private int _waterTile;
    [SerializeField] private int _riverTile;
    [SerializeField] private int _sandTile;
    [SerializeField] private int _dirtTile;
    [SerializeField] private int _stoneTile;
    
    public static TileManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        TileDataManager.Instance.UpdateCount("Grass", _grassTile);
        TileDataManager.Instance.UpdateCount("Water", _waterTile);
        TileDataManager.Instance.UpdateCount("River", _riverTile);
        TileDataManager.Instance.UpdateCount("Sand", _sandTile);
        TileDataManager.Instance.UpdateCount("Dirt", _dirtTile);
        TileDataManager.Instance.UpdateCount("Stone", _stoneTile);

    }

    public void TileCountOnChangeHandler(string newTileTag, string oldTileTag)
    {
        var count = newTileTag switch
        {
            "Grass" => ++_grassTile,
            "Water" => ++_waterTile,
            "River" => ++_riverTile,
            "Sand" => ++_sandTile,
            "Dirt" => ++_dirtTile,
            "Stone" => ++_stoneTile,
            _ => -1
        };
        TileDataManager.Instance.UpdateCount(newTileTag, count);

        count = oldTileTag switch
        {
            "Grass" => --_grassTile,
            "Water" => --_waterTile,
            "River" => --_riverTile,
            "Sand" => --_sandTile,
            "Dirt" => --_dirtTile,
            "Stone" => --_stoneTile,
            _ => -1
        };
        TileDataManager.Instance.UpdateCount(oldTileTag, count);

    }
    
    

    public void RegisterTile(string tileTag)
    {
        switch (tileTag)
        {
            case "Grass":
                _grassTile++;
                break;
            case "Water":
                _waterTile++;
                break;
            case "River":
                _riverTile++;
                break;
            case "Sand":
                _sandTile++;
                break;
            case "Dirt":
                _dirtTile++;
                break;
            case "Stone":
                _stoneTile++;
                break;
        }
    }

}
