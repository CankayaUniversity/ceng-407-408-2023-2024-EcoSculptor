using System;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text resource;
    [SerializeField] private int elementalResource;

    public static EconomyManager Instance;

    private void Awake()
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
        elementalResource = 100;
    }

    private void Update()
    {
        resource.SetText(elementalResource.ToString());
    }

    public void IncreaseResource(int amount)
    {
        elementalResource += amount;
    }
}
