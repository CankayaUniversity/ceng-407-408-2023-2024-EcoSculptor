using System;
using System.Collections;
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

    private bool _foodFlag;      // Whether the tile has food or not
    private GameObject _food;
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

    public bool FoodFlag
    {
        get => _foodFlag;
        set => _foodFlag = value;
    }

    public GameObject Food
    {
        get => _food;
        set => _food = value;
    }


    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
    }

    private void OnEnable()
    {
        TileManager.Instance.RegisterTile(tileMesh.gameObject.tag);
    }

    private void CreateFoodTile(Vector3Int neighborVector)
    {
        var neighborTile = HexGrid.Instance.GetTileAt(neighborVector);
        if (!neighborTile.tileMesh.gameObject.CompareTag("Grass") || neighborTile.FoodFlag) return;
        var position1 = neighborTile.transform.position;

        neighborTile.Food = Instantiate(foods, neighborTile.transform);
        neighborTile.Food.transform.position = new Vector3(position1.x, position1.y - 5, position1.z);
        neighborTile.FoodFlag = true;
        var endPosition = new Vector3(position1.x, position1.y + 1, position1.z);

        StartCoroutine(WaitForSeconds(10f, () =>
        {
            neighborTile.Food.transform.DOMove(endPosition, 5.0f).SetEase(Ease.OutSine);
        }));
        
    }

    private IEnumerator WaitForSeconds(float sec, Action onWaitEnd)
    {
        yield return new WaitForSeconds(sec);
        onWaitEnd?.Invoke();
    }

    public void ControlRiver()
    {
        if(!tileMesh.gameObject.CompareTag("River")) return;
        var neighborsList = HexGrid.Instance.GetNeighboursFor(HexCoords);
        Debug.Log(neighborsList.Count);
        foreach (var neighborVector in neighborsList)
            CreateFoodTile(neighborVector);
    }
}
