
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Price Catalog", menuName = "ScriptableObjects/Price Catalog")]
public class TilePriceCatalog : ScriptableObject
{
    [SerializedDictionary("Tile Tag", "Price")]
    public SerializedDictionary<string, int> priceCatalog;
}