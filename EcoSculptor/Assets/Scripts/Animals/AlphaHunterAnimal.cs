using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.Serialization;

public class AlphaHunterAnimal : Agent
{
    [SerializeField] private float moveSpeed = 4f;
    private Rigidbody rb;

    public GameObject prey;
    [FormerlySerializedAs("classObject")] public PreyAnimal weakestPreyAnimal;
    public HunterAnimal weakestHunterAnimal;
    
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        //Hunter
        Vector3 spawnLocation = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
        transform.localPosition = spawnLocation;
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
            rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
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
        if (other.gameObject.CompareTag("Agent"))
        {
            AddReward(10f);
            weakestPreyAnimal.AddReward(-13f);
            weakestPreyAnimal.EndEpisode();
            weakestHunterAnimal.EndEpisode();
            EndEpisode();
        }
        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-15f);
            weakestPreyAnimal.EndEpisode();
            weakestHunterAnimal.EndEpisode();
            EndEpisode();
        }
        if (other.gameObject.CompareTag("Hunter"))
        {
            AddReward(10f);
            weakestPreyAnimal.EndEpisode();
            weakestHunterAnimal.AddReward(-13f);
            weakestHunterAnimal.EndEpisode();
            EndEpisode();
        }
    }
}