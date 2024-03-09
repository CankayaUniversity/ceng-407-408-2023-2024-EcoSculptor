using UnityEngine;

public class TileChanger : SelectionManager
{
    [SerializeField] private GameObject tilePrefab;

    public override void HandleClick(Vector3 mousePosition)
    {
        base.HandleClick(mousePosition);
        ChangeTile();
    }

    private void ChangeTile()
    {
        Destroy(SelectedHex.TileMesh);
        SelectedHex.TileMesh = Instantiate(tilePrefab, SelectedHex.TileMeshPrent);
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerInput.Instance.PointerHover.AddListener(HandleClick());
        }
    }*/
}