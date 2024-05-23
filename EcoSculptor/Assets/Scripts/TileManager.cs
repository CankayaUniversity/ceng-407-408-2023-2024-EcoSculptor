using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private int _grassTile;
    [SerializeField] private int _waterTile;
    [SerializeField] private int _riverTile;
    [SerializeField] private int _sandTile;
    [SerializeField] private int _dirtTile;
    [SerializeField] private int _stoneTile;

    [Header("Speed Seconds")] 
    [SerializeField] private float waitingSeconds;
    

    private Dictionary<Vector3, WinterHandler> _winterHandlersDict;

    //private List<WinterHandler> _winterHandlers;
    
    public static TileManager Instance;

    public Dictionary<Vector3, WinterHandler> WinterHandlersDict
    {
        get => _winterHandlersDict;
        set => _winterHandlersDict = value;
    }

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

        _winterHandlersDict = new Dictionary<Vector3, WinterHandler>();

        foreach (var w in FindObjectsOfType<WinterHandler>())
        {
            _winterHandlersDict.Add(w.transform.position, w);
        }

    }

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(temp(true));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(temp(false));
        }
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
    
    private IEnumerator temp(bool isWinter)
    {
        var winterHandlers = ShuffleArray(_winterHandlersDict.Values.ToArray());

        if(isWinter)
        {
            foreach (var tile in winterHandlers)
            {
                tile.ChangeTileToWinter();
                yield return new WaitForSeconds(waitingSeconds);
            }

            yield return new WaitForEndOfFrame();
        }
        else
        {
            foreach (var tile in winterHandlers)
            {
                tile.ChangeTileToNormal();
                yield return new WaitForSeconds(waitingSeconds);
            }
        }
        
        
    }
    
    private WinterHandler[] ShuffleArray(WinterHandler[] winterHandlers)
    {
        for (var i = winterHandlers.Length - 1; i > 0; i--)
        {
            var randomIndex = Random.Range(0, i + 1);
            (winterHandlers[i], winterHandlers[randomIndex]) = (winterHandlers[randomIndex], winterHandlers[i]);
        }

        return winterHandlers;
    }
    
    

}
