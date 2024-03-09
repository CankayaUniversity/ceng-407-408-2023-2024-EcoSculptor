using UnityEngine;
using UnityEngine.UIElements;

public class TileChanger : SelectionManager
{
    [SerializeField] private Image buttonImage;

    private GameObject _tilePrefab;
    
    public GameObject TilePrefab
    {
        get => _tilePrefab;
        set => _tilePrefab = value;
    }

    public override void GetHexAndOutline(Vector3 mousePosition)
    {
        base.GetHexAndOutline(mousePosition);
        ChangeTile();
    }

    private void ChangeTile()
    {
        Destroy(SelectedHex.TileMesh);
        SelectedHex.TileMesh = Instantiate(_tilePrefab, SelectedHex.TileMeshPrent);
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerInput.Instance.PointerHover.AddListener(HandleClick());
        }
    }*/
}