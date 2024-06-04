using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text resource;
    [SerializeField] private int elementalResource;

    public static EconomyManager Instance;

    [SerializeField] private TilePriceCatalog _tilePriceCatalog;
    [SerializeField] private DeathPriceCatalog _deathPriceCatalog;
    [SerializeField] private EatingPriceCatalog _eatingPriceCatalog;

    public TilePriceCatalog TilePriceCatalog
    {
        get => _tilePriceCatalog;
        set => _tilePriceCatalog = value;
    }

    public DeathPriceCatalog DeathPriceCatalog
    {
        get => _deathPriceCatalog;
        set => _deathPriceCatalog = value;
    }

    public EatingPriceCatalog EatingPriceCatalog
    {
        get => _eatingPriceCatalog;
        set => _eatingPriceCatalog = value;
    }

    public int ElementalResource
    {
        get => elementalResource;
        set => elementalResource = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //ElementalResource = 100;     //PlayerPrefs
        resource.SetText(ElementalResource.ToString());
    }

    public void IncreaseResource(int amount)
    {
        ElementalResource += amount;
        resource.SetText(ElementalResource.ToString());

    }
    public void DecreaseResource(int amount)
    {
        ElementalResource -= amount;
        resource.SetText(ElementalResource.ToString());

    }
    
}
