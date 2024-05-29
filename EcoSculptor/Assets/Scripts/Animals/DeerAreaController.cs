using System;
using UnityEngine;

public class DeerAreaController : MonoBehaviour
{
    [SerializeField] private PreyAnimal parent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hunter") || other.CompareTag("BearArea"))
        {
            if (parent != null)
            {
                parent.OnHunterEnter();
            }
        }
    }
    
    
}