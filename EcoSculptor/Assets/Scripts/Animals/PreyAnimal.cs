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

    private Collider collideWith;

    private int foodEaten = 0;
    [SerializeField] private FoodManager foodManager;
    public AlphaHunterAnimal strongestHunterAnimal;
    public HunterAnimal weakestHunterAnimal;

    private bool _isDead;
    private bool _isEating;

    public bool IsDead => _isDead;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        PlayAnimation("Movement");
        rb.isKinematic = false;
        rotateSpeed = 6f;
        _isDead = false;
        _isEating = false;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-20f, 20f), 0.4f, Random.Range(-20f, 20f));
        rb.isKinematic = false;
        rotateSpeed = 6f;
        PlayAnimation("Movement");
        foodManager.CreateFood();
        foodEaten = 0;
        foodManager.EpisodeTimerNew();
        _isDead = false;
        _isEating = false;
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
        
        if (!_isEating)
        {
            if (moveForward >= 0)
            {
                var velocity = rb.velocity = transform.forward * moveForward * moveSpeed * Time.deltaTime * 50;
                if(!_isDead)
                    animator.SetFloat("Movement", velocity.magnitude);
            }

            transform.Rotate(0f, moveRotate * rotateSpeed, 0f, Space.Self);
        }
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
            _isEating = true;
            rb.isKinematic = true;
            rotateSpeed = 0;
            collideWith = other;
            animator.Play("deer_deer_eat");
            animator.SetBool("EatingDone",false);
        }
        if (other.gameObject.CompareTag("GrassArea"))
        {
            AddReward(2f);
        }

        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-5f);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.EndEpisode();
            EndEpisode();
        }
    }

    public void RewardFood()
    {
        Destroy(collideWith.gameObject);
        AddReward(12f);
        foodEaten++;
        if (foodEaten == foodManager.foodCount)
        {
            AddReward(15f); // Ekstra ödül ver
            weakestHunterAnimal.AddReward(-5f);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.AddReward(-5f);
            strongestHunterAnimal.EndEpisode();
            EndEpisode();
        }
        _isEating = false;
        rb.isKinematic = false;
        rotateSpeed = 6f;
        animator.SetBool("EatingDone",true);
    }

    public void OnHunterEnter()
    {
        rb.isKinematic = false;
        rotateSpeed = 6f;
        _isEating = false;
    }

    public void PreyDeath()
    {
        if(_isDead) return;
        
        _isDead = true;
        rb.isKinematic = true;
        rotateSpeed = 0;
        animator.Play("deer_deer_death");
    }
}