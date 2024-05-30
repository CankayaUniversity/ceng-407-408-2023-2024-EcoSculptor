using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AnimalSpawnRules", menuName = "Animal Spawn Rules")]
public class AnimalSpawnRules : ScriptableObject
{
    [Header("Spawn Rules")]
    public int riverTileCountForDeer = 2;
    public int dirtTileCountForHorse = 4;
    public int deerCountForWolf = 4;
    public int sandTileForTiger = 4;
    public int stoneTileCountForBear = 1;
    public int animalCountForBear = 10;
    
    [Header("Animal Prefabs")]
    public GameObject deerPrefab;
    public GameObject horsePrefab;
    public GameObject wolfPrefab;
    public GameObject tigerPrefab;
    public GameObject bearPrefab;

    
}