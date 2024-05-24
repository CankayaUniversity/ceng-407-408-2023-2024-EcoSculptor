using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform environmentTransform;
    public int foodCount;

    [SerializeField] private int timeForEpisode;
    private float timeLeft;

    private List<GameObject> spawnedFoodList = new List<GameObject>();

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
        if (spawnedFoodList.Count != 0)
        {
            ClearFood();
        }
        for (int i = 0; i < foodCount; i++)
        {
            int counter = 0;
            bool distanceGood;
            bool alreadyDecr= false;

            GameObject newFood = Instantiate(foodPrefab, environmentTransform, true);

            Vector3 foodLocation = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-20f, 20f));

            if (spawnedFoodList.Count != 0)
            {
                for (int k = 0; k < spawnedFoodList.Count; k++)
                {
                    if (counter < 20)
                    {
                        distanceGood = CheckOverLap(foodLocation, spawnedFoodList[k].transform.localPosition, 5f);
                        if (!distanceGood)
                        {
                            foodLocation = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-20f, 20f));
                            k--;
                            alreadyDecr = true;
                        }

                        distanceGood = CheckOverLap(foodLocation, transform.localPosition, 5f);
                        if (!distanceGood)
                        {
                            foodLocation = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-20f, 20f));
                            if (!alreadyDecr)
                            {
                                k--;
                            }
                        }
                        counter++;
                    }
                    else
                    {
                        k = spawnedFoodList.Count;
                    }
                }
            }
            newFood.transform.localPosition = foodLocation;
            spawnedFoodList.Add(newFood);
        }
    }

    private bool CheckOverLap(Vector3 objectOverLapping, Vector3 alreadyExistingObject, float minDistance)
    {
        float distanceBetweenObjects = Vector3.Distance(objectOverLapping, alreadyExistingObject);
        return distanceBetweenObjects >= minDistance;
    }

    private void ClearFood()
    {
        foreach (GameObject food in spawnedFoodList)
        {
            Destroy(food);
        }
        spawnedFoodList.Clear();
    }

    public bool AllFoodConsumed()
    {
        return spawnedFoodList.Count == 0;
    }

    private void EpisodeTimerNew()
    {
        timeLeft = Time.time + timeForEpisode;
    }

    private void CheckRemainingTime()
    {
        if (Time.time >= timeLeft)
        {
            var animals = FindObjectsOfType<PreyAnimal>();
            foreach (var animal in animals)
            {
                animal.AddReward(-15f);
                animal.EndEpisode();
            }
            ClearFood();
        }
    }
}
