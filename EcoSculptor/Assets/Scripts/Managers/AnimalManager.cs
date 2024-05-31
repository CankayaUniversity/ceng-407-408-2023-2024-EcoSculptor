using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] private AnimalSpawnRules rules;

    [Header("Animal Lists")]
    [SerializeField] private List<GameObject> deers;
    [SerializeField] private List<GameObject> horses;
    [SerializeField] private List<GameObject> tigers;
    [SerializeField] private List<GameObject> wolves;
    [SerializeField] private List<GameObject> bears;
    
    [Header("Animals Spawn Location")] 
    [SerializeField] private List<Transform> spawnLocations;
    
    public static AnimalManager Instance;
    private bool _flag;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        CheckAnimalCount();
    }

    public int TotalAnimalCount()
    {
        return deers.Count + horses.Count + wolves.Count + tigers.Count + bears.Count;
    }

    private void SpawnAnimalType(GameObject prefab, int tileCount, int spawnCount, List<GameObject> animalList)
    {
        if(tileCount == 0) return;
        if (tileCount % spawnCount != 0) return;
        var newAnimal = Instantiate(prefab, transform);
        animalList.Add(newAnimal);
        newAnimal.transform.position = SpawnAtRandomPosition();
    }
    
    private Vector3 SpawnAtRandomPosition()
    {
        var spawnIndex = Random.Range(0, spawnLocations.Count);
        return spawnLocations[spawnIndex].transform.position;
    }

    private void SpawnBearIfNeeded()
    {
        if (TileManager.Instance.StoneTile % rules.stoneTileCountForBear != 0) return;
        
        var totalAnimal = TotalAnimalCount();
        
        if(totalAnimal == 0) return;
        if (totalAnimal % rules.animalCountForBear != 0) return;
        
        var newBear = Instantiate(rules.bearPrefab, transform);
        bears.Add(newBear);
    }

    public void SpawnAnimals()
    {
        SpawnAnimalType(rules.deerPrefab, TileManager.Instance.RiverTile, rules.riverTileCountForDeer, deers);
        SpawnAnimalType(rules.horsePrefab, TileManager.Instance.DirtTile, rules.dirtTileCountForHorse, horses);
        SpawnAnimalType(rules.wolfPrefab, deers.Count, rules.deerCountForWolf, wolves);
        SpawnAnimalType(rules.tigerPrefab, TileManager.Instance.SandTile, rules.sandTileForTiger, tigers);
        SpawnBearIfNeeded();
    }

    private void CheckAnimalCount()
    {
        if (TotalAnimalCount() >= 3 && !_flag)
        {
            TimeManager.Instance.NewRoutine = TimeManager.Instance.StartCoroutine(nameof(TimeManager.LoseControl));
            _flag = true;
        }
    }
    
}
