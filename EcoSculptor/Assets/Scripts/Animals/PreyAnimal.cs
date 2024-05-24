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

    public int foodCountToReward = 5; // Kaç yemek yedikten sonra ekstra ödül verileceği
    private int foodEaten = 0;

    private FoodManager foodManager;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        foodManager = FindObjectOfType<FoodManager>(); // FoodManager'ı bul
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-20f, 20f), 0.46f, Random.Range(-20f, 20f));
        foodManager.CreateFood();
        foodEaten = 0; // Yenilen yemek sayısını sıfırla
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
            AddReward(1f);
            foodEaten++;
            if (foodEaten == foodCountToReward)
            {
                AddReward(10f); // Ekstra ödül ver
            }
            if (foodManager.AllFoodConsumed())
            {
                EndEpisode();
            }
        }
        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
