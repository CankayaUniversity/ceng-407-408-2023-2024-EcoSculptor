
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Price Catalog", menuName = "ScriptableObjects/Tile Price Catalog")]
public class TilePriceCatalog : ScriptableObject
{
    [SerializedDictionary("Tile Tag", "Price")]
    public SerializedDictionary<string, int> priceCatalog;


    public int GetTilePrice(string tag)
    {
        priceCatalog.TryGetValue(tag, out var price);

        return price;
    }
}