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
    private Coroutine _newRoutine;
    private Coroutine _newRoutine2;

    public WinterHandler winterHandler;
    
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
        winterHandler = GetComponentInChildren<WinterHandler>();
    }

    private void OnEnable()
    {
        TileManager.Instance.RegisterTile(tileMesh.gameObject.tag);
    }

    public void StopCoroutine()
    {
        if (_newRoutine != null)
        {
            StopCoroutine(_newRoutine);
        }
        if (_newRoutine2 != null)
        {
            StopCoroutine(_newRoutine2);
        }
    }

    private void CreateFoodTile(Vector3Int neighborVector)
    {
        var neighborTile = HexGrid.Instance.GetTileAt(neighborVector);
        if (!neighborTile.tileMesh.gameObject.CompareTag("Grass") || neighborTile.FoodFlag) return;
        var position1 = neighborTile.transform.position;

        neighborTile.Food = Instantiate(foods, neighborTile.transform);
        neighborTile.Food.SetActive(false);
        
        neighborTile.Food.transform.position = new Vector3(position1.x, position1.y - 10, position1.z);
        neighborTile.FoodFlag = true;
        
        var endPosition = new Vector3(position1.x, position1.y + 0.5f, position1.z);

        _newRoutine = StartCoroutine(WaitForSeconds(10f, () =>
        {
            if (!neighborTile.FoodFlag) return;
            
            neighborTile.Food.SetActive(true);
            neighborTile.Food.transform.DOMove(endPosition, 5.0f).SetEase(Ease.OutSine);
        }));
        
    }

    public void GrowFood(GameObject food) //Geyik otu yediğinde çağırılacak
    {
        if(food.activeInHierarchy)return;
        var pos = food.transform.position;
        var endPosition = new Vector3(pos.x, pos.y + 0.5f, pos.z);
        _newRoutine2 = StartCoroutine(WaitForSeconds(10f, () =>
        {
            if (!food) return;
            pos.y -= 10;
            food.transform.position = pos;
            food.SetActive(true);
            food.transform.DOMove(endPosition, 5.0f).SetEase(Ease.OutSine);
        }));
    }

    private IEnumerator WaitForSeconds(float sec, Action onWaitEnd)
    {
        yield return new WaitForSeconds(sec);
        onWaitEnd?.Invoke();
    }

    public void ControlRiver()
    {
        if (!tileMesh.gameObject.CompareTag("River"))
        {
            StopCoroutine();
            return;
        }
        var neighborsList = HexGrid.Instance.GetNeighboursFor(HexCoords);
        foreach (var neighborVector in neighborsList)
            CreateFoodTile(neighborVector);
    }

    public void ChangeTileToWinter()
    {
        winterHandler.ChangeTileToWinter();
    }

    public void ChangeTileToNormal()
    {
        winterHandler.ChangeTileToNormal();
    }
}
