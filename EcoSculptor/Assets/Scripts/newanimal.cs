using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class newanimal : Agent
{
    [SerializeField] private Transform target;
    public int foodCount;
    public GameObject food;

    [SerializeField] private List<GameObject> spawnedFoodList = new List<GameObject>();
    
    [SerializeField] private float moveSpeed = 4f;
    private Rigidbody rb;

    [SerializeField] private Transform enviromentLocation;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-9f, 9f), 0f, Random.Range(-9f, 9f));
        
        CreateFood();
        //target.localPosition = new Vector3(Random.Range(-9f, 9f), 0f, Random.Range(-9f, 9f));
    }

    private void CreateFood()
    {
        if (spawnedFoodList.Count != 0)
        {
            RemoveFood(spawnedFoodList);
        }
        for (int i = 0; i < foodCount; i++)
        {
            GameObject newFood = Instantiate(food);
            newFood.transform.parent = enviromentLocation;
            
            Vector3 foodLocation= new Vector3(Random.Range(-9f, 9f), 0.2f, Random.Range(-9f, 9f));
            newFood.transform.localPosition = foodLocation;
            spawnedFoodList.Add(newFood);
            Debug.Log("spawned");
            
        }
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
        if (other.gameObject.tag == "nectar")
        {
            Debug.Log("hit");
            spawnedFoodList.Remove(other.gameObject);
            Destroy(other.gameObject);
            AddReward(10f);
            if (spawnedFoodList.Count==0)
            {
                RemoveFood(spawnedFoodList);
                AddReward(5f);
                EndEpisode();
            }
        }
        if (other.gameObject.tag == "boundary")
        {
            RemoveFood(spawnedFoodList);
            AddReward(-15f);
            EndEpisode();
        }
    }
}
