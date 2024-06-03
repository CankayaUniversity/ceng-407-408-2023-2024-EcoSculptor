using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEatingAnim : MonoBehaviour
{
    public PreyAnimal preyParentAnimal;
    public AlphaHunterAnimal alphaParentAnimal;
    public HunterAnimal hunterParentAnimal;
    void Start()
    {
        preyParentAnimal = GetComponentInParent<PreyAnimal>();
        alphaParentAnimal = GetComponentInParent<AlphaHunterAnimal>();
        hunterParentAnimal = GetComponentInParent<HunterAnimal>();
    }

    public void HandleReward()
    {
        preyParentAnimal.RewardFood();
    }

    public void AlphaHunterEat()
    {
        if (alphaParentAnimal.isAgent)
        {
            alphaParentAnimal.EatAgent();
        }
        else
        {
            alphaParentAnimal.EatHunter();
        }
    }
    public void AlphaHunterAttacked()
    {
        if (alphaParentAnimal.isAgent)
        {
            //preyParentAnimal.PreyDeath();
        }
        else
        {
            //hunterParentAnimal.HunterDeath();
        }
    }

    public void HunterEat()
    {
        hunterParentAnimal.EatAgent();
    }

    public void HunterAttacked()
    {
        //preyParentAnimal.PreyDeath();
    }

    public void AlphaDeath()
    {
        alphaParentAnimal.HandleAlphaDeath();
    }

    public void DeerDeathByHunger()
    {
        preyParentAnimal.DestroyOnAnimEnds();
    }

    public void HunterDeathByHunger()
    {
        hunterParentAnimal.DestroyOnAnimEnds();
    }
}
