using UnityEngine;

public class TileChanger : SelectionManager
{
    private GameObject _tilePrefab;

    public GameObject TilePrefab
    {
        get => _tilePrefab;
        set => _tilePrefab = value;
    }

    public override void Outliner(Vector3 mousePosition)
    {
        base.Outliner(mousePosition);
        ChangeTile();
    }

    private void ChangeTile()
    {
        Destroy(SelectedHex.TileMesh);
        SelectedHex.TileMesh = Instantiate(_tilePrefab, SelectedHex.TileMeshPrent);
    }
}