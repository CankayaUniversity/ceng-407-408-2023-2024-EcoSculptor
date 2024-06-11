using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] private AnimalSpawnRules rules;

    [Header("Animal Lists")] 
    [SerializeField] private List<GameObject> deers = new List<GameObject>();
    [SerializeField] private List<GameObject> horses = new List<GameObject>();
    [SerializeField] private List<GameObject> tigers = new List<GameObject>();
    [SerializeField] private List<GameObject> wolves = new List<GameObject>();
    [SerializeField] private List<GameObject> bears = new List<GameObject>();
    
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

    private GameObject SpawnAnimal(GameObject prefab, List<GameObject> animalList)
    {
        var newAnimal = Instantiate(prefab, transform);
        animalList.Add(newAnimal);
        newAnimal.transform.position = SpawnAtRandomPosition();

        return newAnimal;
    }

    private Vector3 SpawnAtRandomPosition()
    {
        int spawnIndex = Random.Range(0, spawnLocations.Count);
        return spawnLocations[spawnIndex].position;
    }

    private void SpawnBearIfNeeded()
    {
        var stoneTile = TileManager.Instance.StoneTile;
        var totalAnimal = TotalAnimalCount();

        if (stoneTile == 0 || totalAnimal == 0 || totalAnimal < 10) return;

        if (stoneTile % rules.stoneTileCountForBear != 0) return;

        if (bears.Count > 0)
        {
            if (totalAnimal % (rules.animalCountForBear * bears.Count) != 0) return;

            if (stoneTile / bears.Count > rules.stoneTileCountForBear)
            {
                SpawnAnimal(rules.bearPrefab, bears);
            }
        }
        else
        {
            SpawnAnimal(rules.bearPrefab, bears);
        }
    }


    public void SpawnAnimals()
    {
        SpawnDeer();
        SpawnHorse();
        SpawnTiger();
        SpawnWolf();
        SpawnBearIfNeeded();
    }

    private void SpawnHorse()
    {
        var dirtTile = TileManager.Instance.DirtTile;
        
        if(dirtTile % rules.dirtTileCountForHorse != 0 || dirtTile <= 0) return;

        
        if (horses.Count == 0)
        {
            var horse = SpawnAnimal(rules.horsePrefab, horses);
            
            var position = horse.transform.position;
            position = new Vector3(position.x, 0, position.z);
            horse.transform.position = position;
            
            return;
        }

        if (dirtTile / horses.Count > rules.dirtTileCountForHorse)
        {
            var horse = SpawnAnimal(rules.horsePrefab, horses);
            
            var position = horse.transform.position;
            position = new Vector3(position.x, 0, position.z);
            horse.transform.position = position;
        }
        
        
    }

    private void SpawnDeer()
    {
        var riverTile = TileManager.Instance.RiverTile;
        
        if(riverTile % rules.riverTileCountForDeer != 0 || riverTile <= 0) return;

        if (deers.Count == 0)
        {
            SpawnAnimal(rules.deerPrefab, deers);
            return;
        }
        
        if (riverTile / deers.Count > rules.riverTileCountForDeer)
        {
            SpawnAnimal(rules.deerPrefab, deers);
        }
        
    }
    
    private void SpawnTiger()
    {
        var sand = TileManager.Instance.SandTile;
        
        if(sand % rules.sandTileForTiger != 0 || sand <= 0) return;

        if (tigers.Count == 0)
        {
            var tiger = SpawnAnimal(rules.tigerPrefab, tigers);
            var position = tiger.transform.position;
            position = new Vector3(position.x, .2f, position.z);
            tiger.transform.position = position;
            return;
        }
        
        if (sand / tigers.Count > rules.sandTileForTiger)
        {
            var tiger = SpawnAnimal(rules.tigerPrefab, tigers);
            var position = tiger.transform.position;
            position = new Vector3(position.x, .2f, position.z);
            tiger.transform.position = position;
        }
    }
    
    private void SpawnWolf()
    {
        var deerCountForWolves = deers.Count;
        
        if(deerCountForWolves % rules.deerCountForWolf != 0 || deerCountForWolves <= 0) return;
        
        if (wolves.Count == 0)
        {
            SpawnAnimal(rules.wolfPrefab, wolves);
            return;
        }
        
        if (deerCountForWolves / wolves.Count > rules.deerCountForWolf)
        {
            SpawnAnimal(rules.wolfPrefab, wolves);
        }
    }

    public void RemoveAnimals(GameObject go)
    {
        switch (go.tag)
        {
            case "DeerBase":
                deers.Remove(go);
                break;
            case "HorseBase":
                horses.Remove(go);
                break;
            case "WolfBase":
                wolves.Remove(go);
                break;
            case "TigerBase":
                tigers.Remove(go);
                break;
            case "BearBase":
                bears.Remove(go);
                break;
        }
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
