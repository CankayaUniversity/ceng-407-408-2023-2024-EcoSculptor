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

    public GameObject prey;
    public PreyAnimal weakestPreyAnimal;
    public HunterAnimal weakestHunterAnimal;

    //private bool isAttacking;
    //private Coroutine _coroutine;

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

    /*private void OnDestroy()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);
    }*/

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
            //if(isAttacking) return;
            var pa = other.gameObject.GetComponentInParent<PreyAnimal>();
            
            alphaAnim.preyParentAnimal = pa;
            
            rb.isKinematic = true;
            rotateSpeed = 0;
            isAgent = true;
            animator.Play("Bear_Attack1");
            //_coroutine = StartCoroutine(AttackCoolDown());
            pa.PreyDeath();
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
            var pa = other.gameObject.GetComponentInParent<HunterAnimal>();
            
            alphaAnim.hunterParentAnimal = pa;
            
            rb.isKinematic = true;
            rotateSpeed = 0;
            animator.Play("Bear_Attack1");
            pa.HunterDeath();
        }
    }

    /*IEnumerator AttackCoolDown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }*/

    public void EatHunter()
    {
        AddReward(10f);
        rb.isKinematic = false;
        rotateSpeed = 6f;
        weakestPreyAnimal.EndEpisode();
        weakestHunterAnimal.AddReward(-13f);
        weakestHunterAnimal.EndEpisode();
        EndEpisode();
    }

    public void EatAgent()
    {
        AddReward(10f);
        rb.isKinematic = false;
        rotateSpeed = 6f;
        isAgent = false;
        weakestPreyAnimal.AddReward(-13f);
        weakestPreyAnimal.EndEpisode();
        weakestHunterAnimal.EndEpisode();
        EndEpisode();
    }
}