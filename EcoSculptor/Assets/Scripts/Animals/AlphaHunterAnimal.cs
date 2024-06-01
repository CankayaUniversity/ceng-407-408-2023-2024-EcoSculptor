using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[SelectionBase]
public class AlphaHunterAnimal : Agent
{
    [Header("Animations")]
    [SerializeField] private Animator animator;

    private HandleEatingAnim alphaAnim;
    
    [Header("Move Speeds")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 6f;
    
    private Rigidbody rb;
    public bool isAgent;

    private Collider _collideWith;

    public GameObject prey;
    public PreyAnimal weakestPreyAnimal;
    public HunterAnimal weakestHunterAnimal;

    public Rigidbody Rb
    {
        get => rb;
        set => rb = value;
    }

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        alphaAnim = GetComponentInChildren<HandleEatingAnim>();
        PlayAnimation("Movement");
    }

    public override void OnEpisodeBegin()
    {
        Vector3 spawnLocation = new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-20f, 20f));
        transform.localPosition = spawnLocation;
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
            var velocity = rb.velocity = transform.forward * moveForward * moveSpeed * Time.deltaTime * 50;
            animator.SetFloat("Movement", velocity.magnitude);
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
        if (other.gameObject.CompareTag("Agent"))
        {
            var pa = other.gameObject.GetComponentInParent<PreyAnimal>();
            _collideWith = other;
            
            alphaAnim.preyParentAnimal = pa;
            
            rb.isKinematic = true;
            rotateSpeed = 0;
            isAgent = true;
            animator.Play("Bear_Attack1");
            pa.PreyDeath();
        }
        if (other.gameObject.CompareTag("RewardArea"))
        {
            AddReward(2f);
        }
        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-6f);
            weakestPreyAnimal.EndEpisode();
            weakestHunterAnimal.EndEpisode();
            EndEpisode();
        }
        if (other.gameObject.CompareTag("Hunter"))
        {
            var pa = other.gameObject.GetComponentInParent<HunterAnimal>();
            _collideWith = other;

            alphaAnim.hunterParentAnimal = pa;
            
            rb.isKinematic = true;
            rotateSpeed = 0;
            animator.Play("Bear_Attack1");
            pa.HunterDeath();
        }
    }

    public void EatHunter()
    {
        rb.isKinematic = false;
        rotateSpeed = 6f;
        //Destroy(_collideWith.transform.parent.gameObject);
        AddReward(6f);
        weakestPreyAnimal.EndEpisode();
        weakestHunterAnimal.AddReward(-5f);
        weakestHunterAnimal.EndEpisode();
        EndEpisode();
    }

    public void EatAgent()
    {
        rb.isKinematic = false;
        rotateSpeed = 6f;
        isAgent = false;
        //Destroy(_collideWith.transform.parent.parent.gameObject);
        AddReward(8f);
        weakestPreyAnimal.AddReward(-5f);
        weakestPreyAnimal.EndEpisode();
        weakestHunterAnimal.EndEpisode();
        EndEpisode();
    }
}