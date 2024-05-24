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
    [SerializeField] private float moveSpeed = 4f;
    private Rigidbody rb;
    
    private int foodEaten = 0;
    [SerializeField] private FoodManager foodManager;
    public AlphaHunterAnimal strongestHunterAnimal;
    public HunterAnimal weakestHunterAnimal;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-20f, 20f), 0.26f, Random.Range(-20f, 20f));
        foodManager.CreateFood();
        foodEaten = 0; // Yenilen yemek sayısını sıfırla
        foodManager.EpisodeTimerNew();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];
        
        if (moveForward >= 0) {
            rb.velocity = transform.forward * moveForward * moveSpeed * Time.deltaTime * 100;
        } else {
            rb.MovePosition(transform.position - transform.forward * Mathf.Abs(moveForward) * moveSpeed * 0.2f * Time.deltaTime);
        }
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);
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
            Destroy(other.gameObject);
            AddReward(10f);
            foodEaten++;
            if (foodEaten == foodManager.foodCount)
            {
                AddReward(5f); // Ekstra ödül ver
                weakestHunterAnimal.AddReward(-5f);
                weakestHunterAnimal.EndEpisode();
                strongestHunterAnimal.AddReward(-5f);
                strongestHunterAnimal.EndEpisode();
                EndEpisode();
            }
        }
        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-15f);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.EndEpisode();
            EndEpisode();
        }
    }
}
