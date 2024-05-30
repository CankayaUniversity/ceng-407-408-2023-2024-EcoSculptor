using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform environmentTransform;
    public int foodCount;

    [SerializeField] private int timeForEpisode;
    private float timeLeft;

    private List<GameObject> _spawnedFoodList = new List<GameObject>();
    
    public PreyAnimal weakestPreyAnimal;
    public AlphaHunterAnimal strongestHunterAnimal;
    public HunterAnimal weakestHunterAnimal;

    private void Start()
    {
        EpisodeTimerNew();
    }

    private void Update()
    {
        CheckRemainingTime();
    }

    public void CreateFood()
    {
        if (_spawnedFoodList.Count != 0)
        {
            ClearFood();
        }
        for (int i = 0; i < foodCount; i++)
        {
            int counter = 0;
            bool distanceGood;
            bool alreadyDecr= false;

            GameObject newFood = Instantiate(foodPrefab, environmentTransform, true);

            Vector3 foodLocation = new Vector3(Random.Range(-20f, 20f), 0.04f, Random.Range(-20f, 20f));

            if (_spawnedFoodList.Count != 0)
            {
                for (int k = 0; k < _spawnedFoodList.Count; k++)
                {
                    if (counter < 20)
                    {
                        distanceGood = CheckOverLap(foodLocation, _spawnedFoodList[k].transform.localPosition, 5f);
                        if (!distanceGood)
                        {
                            foodLocation = new Vector3(Random.Range(-20f, 20f), 0.04f, Random.Range(-20f, 20f));
                            k--;
                            alreadyDecr = true;
                        }

                        distanceGood = CheckOverLap(foodLocation, transform.localPosition, 5f);
                        if (!distanceGood)
                        {
                            foodLocation = new Vector3(Random.Range(-20f, 20f), 0.04f, Random.Range(-20f, 20f));
                            if (!alreadyDecr)
                            {
                                k--;
                            }
                        }
                        counter++;
                    }
                    else
                    {
                        k = _spawnedFoodList.Count;
                    }
                }
            }
            newFood.transform.localPosition = foodLocation;
            _spawnedFoodList.Add(newFood);
        }
    }

    private bool CheckOverLap(Vector3 objectOverLapping, Vector3 alreadyExistingObject, float minDistance)
    {
        float distanceBetweenObjects = Vector3.Distance(objectOverLapping, alreadyExistingObject);
        return distanceBetweenObjects >= minDistance;
    }

    private void ClearFood()
    {
        foreach (GameObject food in _spawnedFoodList)
        {
            Destroy(food);
        }
        _spawnedFoodList.Clear();
    }

    internal void EpisodeTimerNew()
    {
        timeLeft = Time.time + timeForEpisode;
    }

    private void CheckRemainingTime()
    {
        if (Time.time >= timeLeft)
        {
            weakestPreyAnimal.AddReward(-5f);
            weakestHunterAnimal.AddReward(-5f);
            strongestHunterAnimal.AddReward(-5f);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.EndEpisode();
            weakestPreyAnimal.EndEpisode();
        }
    }
}
