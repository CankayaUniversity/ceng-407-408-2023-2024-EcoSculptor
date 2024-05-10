using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewAnimal : Agent
{
    [SerializeField] private Transform target;
    public int foodCount;
    public GameObject food;

    [SerializeField] private List<GameObject> spawnedFoodList = new List<GameObject>();
    
    [SerializeField] private float moveSpeed = 4f;
    private Rigidbody rb;

    [SerializeField] private Transform enviromentLocation;

    
    //time keeping variables
    [SerializeField] private int timeForEpisode;
    private float timeLeft;
    
    //Enemy Agent
    public HunterAnimal classObject;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-9f, 9f), 0f, Random.Range(-9f, 9f));
        
        CreateFood();
        
        EpisodeTimerNew();
    }

    private void Update()
    {
        CheckRemainingTime();
    }

    private void CreateFood()
    {
        if (spawnedFoodList.Count != 0)
        {
            RemoveFood(spawnedFoodList);
        }
        for (int i = 0; i < foodCount; i++)
        {
            int counter = 0;
            bool distanceGood;
            bool alreadyDecr= false;
            
            GameObject newFood = Instantiate(food, enviromentLocation, true);

            Vector3 foodLocation= new Vector3(Random.Range(-9f, 9f), -0.43f, Random.Range(-9f, 9f));

            if (spawnedFoodList.Count != 0)
            {
                for (int k = 0; k < spawnedFoodList.Count; k++)
                {
                    if (counter < 10)
                    {
                        distanceGood = CheckOverLap(foodLocation, spawnedFoodList[k].transform.localPosition, 5f);
                        if (distanceGood == false)
                        {
                            foodLocation= new Vector3(Random.Range(-9f, 9f), -0.43f, Random.Range(-9f, 9f));
                            k--;
                            alreadyDecr = true;
                        }
                        
                        distanceGood = CheckOverLap(foodLocation, transform.localPosition, 5f);
                        if (distanceGood == false)
                        {
                            foodLocation= new Vector3(Random.Range(-9f, 9f), -0.43f, Random.Range(-9f, 9f));
                            if (alreadyDecr == false)
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

    private bool CheckOverLap(Vector3 objectOverLapping,Vector3 alreadyExistingObject, float minDistance)
    {
        float distanceBetweenObjects = Vector3.Distance(objectOverLapping, alreadyExistingObject);
        if (minDistance <= distanceBetweenObjects)
        {
            return true;
        }

        return false;
    }

    private void RemoveFood(List<GameObject> toBeDeletedGameObject)
    {
        foreach (GameObject i in toBeDeletedGameObject)
        {
            Destroy(i.gameObject);
        }
        toBeDeletedGameObject.Clear();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];
        
        rb.MovePosition(transform.position + transform.forward*moveForward* moveSpeed*Time.deltaTime);
        transform.Rotate(0f,moveRotate*moveSpeed,0f,Space.Self);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("nectar"))
        {
            var parent = other.transform.parent;
            spawnedFoodList.Remove(parent.gameObject);
            Destroy(parent.gameObject);
            AddReward(10f);
            if (spawnedFoodList.Count==0)
            {
                RemoveFood(spawnedFoodList);
                AddReward(5f);
                classObject.AddReward(-5f);
                classObject.EndEpisode();
                EndEpisode();
            }
        }
        if (other.gameObject.CompareTag("boundary"))
        {
            RemoveFood(spawnedFoodList);
            AddReward(-15f);
            classObject.EndEpisode();
            EndEpisode();
        }
    }

    private void EpisodeTimerNew()
    {
        timeLeft = Time.time + timeForEpisode;
    }

    private void CheckRemainingTime()
    {
        if (Time.time >= timeLeft)
        {
            AddReward(-15f);
            classObject.AddReward(-15f);
            RemoveFood(spawnedFoodList);
            classObject.EndEpisode();
            EndEpisode();
        }
    }
    
}
