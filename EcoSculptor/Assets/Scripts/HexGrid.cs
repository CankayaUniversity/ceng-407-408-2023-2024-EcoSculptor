using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    private Dictionary<Vector3Int, Hex> _hexTileDict = new Dictionary<Vector3Int, Hex>();

    private Dictionary<Vector3Int, List<Vector3Int>> _hexTileNeighboursDict =
        new Dictionary<Vector3Int, List<Vector3Int>>();
    
    public static HexGrid Instance;
    
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
        foreach (var hex in FindObjectsOfType<Hex>())
        {
            _hexTileDict[hex.HexCoords] = hex;
        }
    }

    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        _hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    {
        if (!_hexTileDict.ContainsKey(hexCoordinates)) 
            return new List<Vector3Int>();

        if (_hexTileNeighboursDict.ContainsKey(hexCoordinates)) 
            return _hexTileNeighboursDict[hexCoordinates];

        var neighbours = new List<Vector3Int>();

        foreach (var direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            Vector3Int neighbourCoords = hexCoordinates + direction;
            
            if (_hexTileDict.ContainsKey(neighbourCoords))
            {
                neighbours.Add(neighbourCoords);
            }
        }

        _hexTileNeighboursDict[hexCoordinates] = neighbours;
        return neighbours;
    }
}