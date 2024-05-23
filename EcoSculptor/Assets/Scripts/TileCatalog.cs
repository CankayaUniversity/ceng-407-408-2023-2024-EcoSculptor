
using AYellowpaper.SerializedCollections;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu(fileName = "TileCatalog", menuName = "ScriptableObjects/Tiles")]
public class TileCatalog : ScriptableObject
{
    [SerializedDictionary("Tile Type", "Tile")]
    public SerializedDictionary<TileTypeEnum, GameObject> tileCatalog;
}