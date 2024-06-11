using System;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
public class PreyAnimal : Agent
{
    [Header("Animations")] [SerializeField]
    private Animator animator;

    [Header("Speeds")] [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 6f;
    
    [Header("Hunger")]
    [SerializeField] private Hunger hunger;
    [SerializeField] private float hungerAgainTimer = 60;
    private bool _isHungry;

    
    [Header("Colliders")]
    [SerializeField] private Collider deerArea;
    private Rigidbody rb;

    private Collider collideWith;

    private int foodEaten = 0;
    //[SerializeField] private FoodManager foodManager;
    //public AlphaHunterAnimal strongestHunterAnimal;
    //public HunterAnimal weakestHunterAnimal;

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
        hunger.enabled = true;
        _isHungry = true;

        hunger.OnAnimalDeathByHunger += PreyDeathByHunger;
    }

    private void OnDestroy()
    {
        AnimalManager.Instance.RemoveAnimals(this.gameObject);
    }
    /*public override void OnEpisodeBegin()
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
    }*/

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    private void PlayAnimation(string stateName)
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
            if(!_isHungry) return;
            DeerEatsFood(other);
            other.enabled = false;
        }
        /*if (other.gameObject.CompareTag("GrassArea"))
        {
            AddReward(5f);
        }

        if (other.gameObject.CompareTag("boundary"))
        {
            AddReward(-10f);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.EndEpisode();
            EndEpisode();
        }*/
    }

    public void CloseColliders()
    {
        var componentList = GetComponentsInChildren<Collider>();
        foreach (var collider in componentList)
        {
            collider.enabled = false;
        }
    }

    private void DeerEatsFood(Collider other)
    {
        _isEating = true;
        rb.isKinematic = true;
        rotateSpeed = 0;
        collideWith = other;
        collideWith.enabled = false;
        
        //Animation
        animator.Play("deer_deer_eat");
        animator.SetBool("EatingDone",false);
    }
    
    
    [Header("Gain Elemental When Die")] 
    [SerializeField] private int elementalResourceAmount = 100;

    public void RewardFood()
    {
        if (collideWith)
        {
            collideWith.gameObject.SetActive(false);
            var hex = collideWith.GetComponentInParent<Hex>();
            hex.GrowFood(collideWith.gameObject);
            rotateSpeed = 6f;
            collideWith.enabled = true;
            rb.isKinematic = false;
            _isEating = false;
            animator.SetBool("EatingDone", true);
            
            EconomyManager.Instance.IncreaseResource(elementalResourceAmount);
            
            RestoreHunger();
        }
            
        
        /*AddReward(5f);
        foodEaten++;
        if (foodEaten == foodManager.foodCount)
        {
            AddReward(15f); // Ekstra ödül ver
            weakestHunterAnimal.AddReward(-5f);
            weakestHunterAnimal.EndEpisode();
            strongestHunterAnimal.AddReward(-5f);
            strongestHunterAnimal.EndEpisode();
            EndEpisode();
        }*/
        
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

    public void DestroyOnAnimEnds()
    {
        if (isPreyDeathByHunger)
            Destroy(gameObject);
    }
    
    public void OnHunterEnter()
    {
        rb.isKinematic = false;
        rotateSpeed = 6f;
        _isEating = false;
        if(collideWith)
            collideWith.enabled = true;
    }

    public void PreyDeath()
    {
        if(_isDead) return;
        
        _isDead = true;
        rb.isKinematic = true;
        rotateSpeed = 0;
        animator.Play("deer_deer_death");
    }

    private bool isPreyDeathByHunger;
    public void PreyDeathByHunger(Hunger hunger)
    {
        isPreyDeathByHunger = true;
        PreyDeath();
    }
}