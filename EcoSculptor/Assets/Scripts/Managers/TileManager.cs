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
    [SerializeField] private int grassTile;
    [SerializeField] private int waterTile;
    [SerializeField] private int riverTile;
    [SerializeField] private int sandTile;
    [SerializeField] private int dirtTile;
    [SerializeField] private int stoneTile;

    [Header("Speed Seconds")] 
    [SerializeField] private float waitingSeconds;
    

    private Dictionary<Vector3, Hex> _winterHandlersDict;

    public int GrassTile => grassTile;
    public int WaterTile => waterTile;
    public int RiverTile => riverTile;
    public int SandTile => sandTile;
    public int DirtTile => dirtTile;
    public int StoneTile => stoneTile;

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
        TileDataManager.Instance.UpdateCount("Grass", grassTile);
        TileDataManager.Instance.UpdateCount("Water", waterTile);
        TileDataManager.Instance.UpdateCount("River", riverTile);
        TileDataManager.Instance.UpdateCount("Sand", sandTile);
        TileDataManager.Instance.UpdateCount("Dirt", dirtTile);
        TileDataManager.Instance.UpdateCount("Stone", stoneTile);

        _winterHandlersDict = new Dictionary<Vector3, Hex>();

        foreach (var h in FindObjectsOfType<Hex>())
        {
            _winterHandlersDict.Add(h.transform.position, h);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HandleWinter(true));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(HandleWinter(false));
        }
    }

    public void TileCountOnChangeHandler(string newTileTag, string oldTileTag)
    {
        var count = newTileTag switch
        {
            "Grass" => ++grassTile,
            "Water" => ++waterTile,
            "River" => ++riverTile,
            "Sand" => ++sandTile,
            "Dirt" => ++dirtTile,
            "Stone" => ++stoneTile,
            _ => -1
        };
        TileDataManager.Instance.UpdateCount(newTileTag, count);

        count = oldTileTag switch
        {
            "Grass" => --grassTile,
            "Water" => --waterTile,
            "River" => --riverTile,
            "Sand" => --sandTile,
            "Dirt" => --dirtTile,
            "Stone" => --stoneTile,
            _ => -1
        };
        TileDataManager.Instance.UpdateCount(oldTileTag, count);
    }
    
    

    public void RegisterTile(string tileTag)
    {
        switch (tileTag)
        {
            case "Grass":
                grassTile++;
                break;
            case "Water":
                waterTile++;
                break;
            case "River":
                riverTile++;
                break;
            case "Sand":
                sandTile++;
                break;
            case "Dirt":
                dirtTile++;
                break;
            case "Stone":
                stoneTile++;
                break;
        }
    }
    
    public IEnumerator HandleWinter(bool isWinter)
    {
        var winterHandlers = ShuffleArray(_winterHandlersDict.Values.ToArray());

        
        if(isWinter)
        {
            foreach (var tile in winterHandlers)
            {
                if(tile.winterHandler)
                    tile.ChangeTileToWinter();
                yield return new WaitForSeconds(waitingSeconds);
            }

            yield return new WaitForEndOfFrame();
        }
        else
        {
            foreach (var tile in winterHandlers)
            {
                if(tile.winterHandler)
                    tile.ChangeTileToNormal();
                yield return new WaitForSeconds(waitingSeconds);
            }
        }
        
        
    }
    
    private Hex[] ShuffleArray(Hex[] hexes)
    {
        for (var i = hexes.Length - 1; i > 0; i--)
        {
            var randomIndex = Random.Range(0, i + 1);
            (hexes[i], hexes[randomIndex]) = (hexes[randomIndex], hexes[i]);
        }

        return hexes;
    }
    
    

}
