using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Death Price Catalog", menuName = "ScriptableObjects/Death Price Catalog")]

public class DeathPriceCatalog : ScriptableObject
{
    [SerializedDictionary("Animal Tag", "Price")]
    public SerializedDictionary<string, int> priceCatalog;
}
