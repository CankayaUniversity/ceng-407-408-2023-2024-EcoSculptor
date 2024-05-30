using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[SelectionBase]
public class PreyAnimal : Agent
{
    [Header("Animations")] [SerializeField]
    private Animator animator;

    [Header("Speeds")] [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 6f;

    [SerializeField] private Collider deerArea;
    private Rigidbody rb;

    private Collider colliderWith;

    private int foodEaten = 0;
    [SerializeField] private FoodManager foodManager;
    public AlphaHunterAnimal strongestHunterAnimal;
    public HunterAnimal weakestHunterAnimal;

    private bool _isDead;

    public bool IsDead => _isDead;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        PlayAnimation("Movement");
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-20f, 20f), 0.26f, Random.Range(-20f, 20f));
        rb.isKinematic = false;
        rotateSpeed = 6f;
        PlayAnimation("Movement");
        foodManager.CreateFood();
        foodEaten = 0;
        foodManager.EpisodeTimerNew();
        _isDead = false;
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

        if (moveForward >= 0)
        {
            var velocity = rb.velocity = transform.forward * moveForward * moveSpeed * Time.deltaTime * 50;
            if(!_isDead)
                animator.SetFloat("Movement", velocity.magnitude);
        }
        else
        {
            rb.velocity = -transform.forward * Mathf.Abs(moveForward) * 0.2f * Time.deltaTime;
        }

        transform.Rotate(0f, moveRotate * rotateSpeed, 0f, Space.Self);
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
            rb.isKinematic = true;
            rotateSpeed = 0;
            colliderWith = other;
            animator.Play("deer_deer_eat");
        }

        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-15f);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.EndEpisode();
            EndEpisode();
        }
    }

    public void RewardFood()
    {
        Destroy(colliderWith.gameObject);
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

        rb.isKinematic = false;
        rotateSpeed = 6f;
    }

    public void OnHunterEnter()
    {

        rb.isKinematic = false;
        rotateSpeed = 6f;
        Debug.Log("Enter");
    }

    public void PreyDeath()
    {
        if(_isDead) return;
        
        Debug.Log("Prey Death");
        _isDead = true;
        rb.isKinematic = true;
        rotateSpeed = 0;
        animator.Play("deer_deer_death");
    }
}