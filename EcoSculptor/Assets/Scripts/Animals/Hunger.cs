
using System;
using System.Collections;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    [Header("Hunger")] 
    [SerializeField] private int maxHunger = 90;
    [SerializeField] private float currentHunger;
    [SerializeField] private float hungerDepletionRate = 1;

    private Coroutine depleteHungerRoutine;

    public Action<Hunger> OnAnimalDeathByHunger;

    private void OnEnable()
    {
        currentHunger = maxHunger;
        depleteHungerRoutine = StartCoroutine(DepleteHunger());
    }

    private void OnDisable()
    {
        if(depleteHungerRoutine != null)
            StopCoroutine(depleteHungerRoutine);
    }

    private IEnumerator DepleteHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            currentHunger -= hungerDepletionRate;
            if (currentHunger <= 0)
            {
                currentHunger = 0;
                OnAnimalDeathByHunger?.Invoke(this);
                this.enabled = false;
                yield break;
            }
        }
    }

    public void ResetCurrentHunger()
    {
        currentHunger = maxHunger;
    }
}