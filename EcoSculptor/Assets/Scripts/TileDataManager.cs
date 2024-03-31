using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileDataManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _grassCountText;
    [SerializeField] private TMP_Text _waterCountText;
    [SerializeField] private TMP_Text _riverCountText;
    [SerializeField] private TMP_Text _sandCountText;
    [SerializeField] private TMP_Text _dirtCountText;
    [SerializeField] private TMP_Text _stoneCountText;
    
    public static TileDataManager Instance;

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
        InitilizeTexts();
    }

    private void InitilizeTexts()
    {
        _grassCountText.text = 0.ToString();
        _waterCountText.text = 0.ToString();
        _riverCountText.text = 0.ToString();
        _sandCountText.text = 0.ToString();
        _dirtCountText.text = 0.ToString();
        _stoneCountText.text = 0.ToString();
    }

    public void UpdateCount(string tileTag, int count)
    {
        switch (tileTag)
        {
            case "Grass":
                _grassCountText.text = count.ToString();
                break;
            case "Water":
                _waterCountText.text = count.ToString();
                break;
            case "River":
                _riverCountText.text = count.ToString();
                break;
            case "Sand":
                _sandCountText.text = count.ToString();
                break;
            case "Dirt":
                _dirtCountText.text = count.ToString();
                break;
            case "Stone":
                _stoneCountText.text = count.ToString();
                break;
        }
    }
}
