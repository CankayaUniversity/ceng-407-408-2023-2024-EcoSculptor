using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRiverDirections : MonoBehaviour
{
     [SerializeField] private bool[] sides = new bool[6];

     public bool[] Sides => sides;
}
