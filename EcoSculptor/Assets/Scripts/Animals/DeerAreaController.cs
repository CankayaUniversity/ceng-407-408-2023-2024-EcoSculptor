using System;
using UnityEngine;

public class DeerAreaController : MonoBehaviour
{
    [SerializeField] private PreyAnimal parent;
    
    private void OnTriggerEnter(Collider other)
    {
        if(parent.IsDead) return;
        
        if (other.CompareTag("WolfArea") || other.CompareTag("BearArea"))
        {
            if (parent != null)
            {
                parent.OnHunterEnter();
            }
        }
    }
    
    
}