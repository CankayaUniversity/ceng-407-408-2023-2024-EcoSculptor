using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PreyAnimal : Agent
{
    [Header("Animations")]
    [SerializeField] private Animator animator;
    
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
    
    //Enemys Agent
    [FormerlySerializedAs("classObject")] public HunterAnimal weakestHunterAnimal;
    public AlphaHunterAnimal strongestHunterAnimal;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0.46f, Random.Range(-4f, 4f));
        
        CreateFood();
        
        EpisodeTimerNew();
        
        PlayAnimation("Movement");
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

            Vector3 foodLocation= new Vector3(Random.Range(-20f, 20f), -0.41f, Random.Range(-20f, 20f));

            if (spawnedFoodList.Count != 0)
            {
                for (int k = 0; k < spawnedFoodList.Count; k++)
                {
                    if (counter < 20)
                    {
                        distanceGood = CheckOverLap(foodLocation, spawnedFoodList[k].transform.localPosition, 5f);
                        if (distanceGood == false)
                        {
                            foodLocation= new Vector3(Random.Range(-20f, 20f), -0.41f, Random.Range(-20f, 20f));
                            k--;
                            alreadyDecr = true;
                        }
                        
                        distanceGood = CheckOverLap(foodLocation, transform.localPosition, 5f);
                        if (distanceGood == false)
                        {
                            foodLocation= new Vector3(Random.Range(-20f, 20f), -0.41f, Random.Range(-20f, 20f));
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
    
    public void PlayAnimation(string stateName)
    {
        animator.CrossFadeInFixedTime(stateName, 0f, 0, 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];
        
        if (moveForward >= 0) {
            var velocity = rb.velocity = transform.forward * moveForward * moveSpeed * Time.deltaTime;
            animator.SetFloat("Movement", velocity.magnitude);
        } else {
            rb.MovePosition(transform.position - transform.forward * Mathf.Abs(moveForward) * moveSpeed * 0.2f * Time.deltaTime);
        }
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
                weakestHunterAnimal.AddReward(-5f);
                weakestHunterAnimal.EndEpisode();
                strongestHunterAnimal.AddReward(-5f);
                strongestHunterAnimal.EndEpisode();
                EndEpisode();
            }
        }
        if (other.gameObject.CompareTag("boundary"))
        {
            RemoveFood(spawnedFoodList);
            AddReward(-15f);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.EndEpisode();
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
            weakestHunterAnimal.AddReward(-15f);
            strongestHunterAnimal.AddReward(-15f);
            RemoveFood(spawnedFoodList);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.EndEpisode();
            EndEpisode();
        }
    }
}
