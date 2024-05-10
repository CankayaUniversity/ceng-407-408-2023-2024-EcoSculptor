using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActivitySetter : MonoBehaviour
{
    [SerializeField] private List<Transform> openList;
    [SerializeField] private List<Transform> closeList;

    public void OpenObjects()
    {
        foreach (var o in openList)
        {
            o.gameObject.SetActive(true);
        }
    }
    
    public void CloseObjects()
    {
        foreach (var c in closeList)
        {
            c.gameObject.SetActive(false);
        }
    }
}