using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private List<Transform> optionList;
    
    public void HandleOptionClick(Transform option)
    {
        foreach (var o in optionList.Where(o => option != o))
        {
            o.gameObject.SetActive(false);
        }
    }
}
