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
public class HunterAnimal : Agent
{
    [Header("Animations")]
    [SerializeField] private Animator animator;
    
    [Header("Move Speeds")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 6f;
    
    private Rigidbody rb;
    private HandleEatingAnim hunterAnim;
    private bool isDead;

    public GameObject prey;
    public PreyAnimal weakestPreyAnimal;
    public AlphaHunterAnimal strongestHunterAnimal;
    
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        hunterAnim = GetComponentInChildren<HandleEatingAnim>();
        PlayAnimation("Movement");
    }

    public override void OnEpisodeBegin()
    {
        //Hunter
        Vector3 spawnLocation = new Vector3(Random.Range(-20f, 20f), 0.15f, Random.Range(-20f, 20f));
        transform.localPosition = spawnLocation;
        rb.isKinematic = false;
        rotateSpeed = 6f;
        PlayAnimation("Movement");
        Debug.Log("YÜRÜMESİ LAZIM");
        isDead = false;
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
            if(!isDead)
                animator.SetFloat("Movement", velocity.magnitude);
        } else {
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
        if (other.gameObject.CompareTag("Agent"))
        {
            var pah = other.gameObject.GetComponentInParent<PreyAnimal>();
            
            hunterAnim.preyParentAnimal = pah;
            rb.isKinematic = true;
            rotateSpeed = 0;
            animator.Play("dog_test_wolf-attack");
            pah.PreyDeath();
        }
        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-15f);
            weakestPreyAnimal.EndEpisode();
            strongestHunterAnimal.EndEpisode();
            EndEpisode();
        }
    }

    public void EatAgent()
    {
        AddReward(10f);
        weakestPreyAnimal.AddReward(-13f);
        rb.isKinematic = false;
        rotateSpeed = 6f;
        weakestPreyAnimal.EndEpisode();
        strongestHunterAnimal.EndEpisode();
        EndEpisode();
    }

    public void HunterDeath()
    {
        if (isDead)
        {
            return;
        }
        Debug.Log("ölüm başladı");
        isDead = true;
        rb.isKinematic = true;
        rotateSpeed = 0;
        animator.SetTrigger("Death");
        animator.Play("dog_test_wolf-death");
        Debug.Log("ölüm bitti");
    }
}
