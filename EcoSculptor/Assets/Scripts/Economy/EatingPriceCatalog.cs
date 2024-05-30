using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Eating Price Catalog", menuName = "ScriptableObjects/Eating Price Catalog")]
public class EatingPriceCatalog : ScriptableObject
{
    [SerializedDictionary("Animal Tag", "Price")]
    public SerializedDictionary<string, int> priceCatalog;
}
