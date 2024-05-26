using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [Header("Component Elements")] 
    [SerializeField] private Outline outline;
    [SerializeField] private Transform tileMeshParent;
    [SerializeField] private GameObject tileMesh;
    [SerializeField] private GameObject foods;

    private bool _flag;
    private bool _isSafe = true;


    private HexCoordinates _hexCoordinates;
    
    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    public Outline Outline
    {
        get => outline;
        set => outline = value;
    }


    public Transform TileMeshParent
    {
        get => tileMeshParent;
        set => tileMeshParent = value;
    }

    public GameObject TileMesh
    {
        get => tileMesh;
        set => tileMesh = value;
    }


    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
    }

    private void OnEnable()
    {
        TileManager.Instance.RegisterTile(tileMesh.gameObject.tag);
        if(tileMesh.gameObject.tag != "River") return;
        var neighborsList = HexGrid.Instance.GetNeighboursFor(HexCoords);
        foreach (var neighborVector in neighborsList)
        {
            if(!_flag)
                _flag = CreateFoodTile(neighborVector);
            else 
                break;
        }
        
    }

    private bool CreateFoodTile(Vector3Int neighborVector)
    {
        var tile = HexGrid.Instance.GetTileAt(neighborVector);
        if (!tile.tileMesh.gameObject.CompareTag("Grass")) return false;
        if(!_isSafe) return false;

        _isSafe = false;
        var newTile= Instantiate(foods, transform);
        var position1 = tileMesh.transform.position;
        newTile.transform.position = new Vector3(position1.x, position1.y - 5, position1.z);
        var endY = new Vector3(position1.x, position1.y + 2, position1.z);
        
        transform.DOMove(endY, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _isSafe = true;
        });
        Debug.Log("instantiated: "+ newTile.transform.position);

        return true;
    }
}
