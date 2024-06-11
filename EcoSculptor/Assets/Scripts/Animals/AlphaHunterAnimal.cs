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
    
    [Header("Hunger")]
    [SerializeField] private Hunger hunger;
    [SerializeField] private float hungerAgainTimer = 60;
    private bool _isHungry;
    
    private Rigidbody rb;
    public bool isAgent;

    [Header("Collider")]
    private Collider _collideWith;
    private Coroutine EatRoutine;
    private bool _isEating;

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


        hunger.OnAnimalDeathByHunger += OnDeathByHunger;
        hunger.enabled = true;
        _isEating = false;
        _isHungry = true;
    }

    protected override void OnDisable()
    {
        if(EatRoutine != null)
            StopCoroutine(EatRoutine);
    }

    private void OnDestroy()
    {
        AnimalManager.Instance.RemoveAnimals(this.gameObject);
    }
    /*public override void OnEpisodeBegin()
    {
        Vector3 spawnLocation = new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-20f, 20f));
        transform.localPosition = spawnLocation;
    }*/
    
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

    /*public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }*/
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Agent"))
        {
            if (_isEating || !_isHungry) return;
            
            var pa = other.gameObject.GetComponentInParent<PreyAnimal>();
            _collideWith = other;
            
            alphaAnim.preyParentAnimal = pa;
            _isEating = true;
            rb.isKinematic = true;
            rotateSpeed = 0;
            isAgent = true;
            animator.Play("Bear_Attack1");
            pa.CloseColliders();
            pa.PreyDeath();
        }
        /*if (other.gameObject.CompareTag("RewardArea"))
        {
            AddReward(2f);
        }
        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-10f);
            weakestPreyAnimal.EndEpisode();
            weakestHunterAnimal.EndEpisode();
            EndEpisode();
        }*/
        if (other.gameObject.CompareTag("Hunter"))
        {
            if (_isEating || !_isHungry) return;
            
            var pa = other.gameObject.GetComponentInParent<HunterAnimal>();
            _collideWith = other;

            alphaAnim.hunterParentAnimal = pa;
            _isEating = true;
            rb.isKinematic = true;
            rotateSpeed = 0;
            animator.Play("Bear_Attack1");
            pa.HunterDeath();
        }
    }

    
    [Header("Gain Elemental When Die")] 
    [SerializeField] private int elementalResourceAmount = 100;

    public void EatHunter()
    {
        rb.isKinematic = false;
        rotateSpeed = 6f;
        if(_collideWith)
            Destroy(_collideWith.transform.parent.gameObject);
        
        RestoreHunger();
        _isEating = false;

        EconomyManager.Instance.IncreaseResource(elementalResourceAmount);
        /*AddReward(5f);
        weakestPreyAnimal.EndEpisode();
        weakestHunterAnimal.AddReward(-5f);
        weakestHunterAnimal.EndEpisode();
        EndEpisode();*/
    }

    public void EatAgent()
    {
        rb.isKinematic = false;
        rotateSpeed = 6f;
        isAgent = false;
        if(_collideWith)
            Destroy(_collideWith.transform.parent.parent.gameObject);
        
        RestoreHunger();
        
        EconomyManager.Instance.IncreaseResource(elementalResourceAmount);

        RestoreHunger();
        _isEating = false;

        /*AddReward(5f);
        weakestPreyAnimal.AddReward(-5f);
        weakestPreyAnimal.EndEpisode();
        weakestHunterAnimal.EndEpisode();
        EndEpisode();*/
    }
    
    private void RestoreHunger()
    {
        //Hunger
        hunger.ResetCurrentHunger();
        hunger.enabled = false;
        _isHungry = false;
        Invoke(nameof(HungerAgain), hungerAgainTimer);
    }
    
    private void HungerAgain()
    {
        hunger.enabled = true;
        _isHungry = true;
    }
    
    public void OnDeathByHunger(Hunger hunger)
    {
        animator.Play("Bear_Death");
    }

    public void HandleAlphaDeath()
    {
        rb.isKinematic = true;
        rotateSpeed = 0;
        Destroy(gameObject);
    }
}