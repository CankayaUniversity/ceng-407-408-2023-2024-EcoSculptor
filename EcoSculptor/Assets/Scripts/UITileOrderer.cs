using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[RequireComponent(typeof(ToggleGameObject))]
public class UITileOrderer : MonoBehaviour
{
    [Header("Ordering Properties")]
    
    [Tooltip("FromMiddle: Use each option\nSideBySide: Use SpaceXDirection\nUpward: Use SpaceYDirection")]
    [SerializeField] private Locater locater;
    [SerializeField] private float startingXPos;
    [SerializeField] private float spaceXDirection = 150f;
    [SerializeField] private float spaceYDirection = 80f;

    private Tween _newTween;
    private void OnEnable()
    {
        switch (locater)
        {
            case Locater.UPWARD:
                OderTilesUpward();
                break;
            
            case Locater.SIDEBYSIDE:
                OrderTilesSideBySide();
                break;
            
            case Locater.FROMMIDDLE:
                OrderTilesFromMiddle();
                break;
        }
    }

    private void OnDisable()
    {
        _newTween?.Kill();
        gameObject.SetActive(false);

    }

    private void OrderTilesFromMiddle()
    {            
        var iteration = 1;
        foreach (RectTransform child in transform)
        {
            child.anchoredPosition = Vector2.zero;
            var newPos = new Vector3(startingXPos + (spaceXDirection * iteration++), spaceYDirection, 0);
            child.DOLocalMove(newPos, 0.2f).SetEase(Ease.OutBack);
        }
    }

    private void OrderTilesSideBySide()
    {
        var iteration = 1;
        foreach (RectTransform child in transform)
        {
            child.anchoredPosition = Vector2.zero;
            var newPos = new Vector3(spaceXDirection * iteration++, 0, 0);
            child.DOLocalMove(newPos, 0.2f).SetEase(Ease.OutBack);
        }
    }

    private void OderTilesUpward()
    {
        var iteration = 1;
        foreach (RectTransform child in transform)
        {
            child.anchoredPosition = Vector2.zero;
            var newPos = new Vector3(0, spaceYDirection * iteration++, 0);
            child.DOLocalMove(newPos, 0.2f).SetEase(Ease.OutBack);
        }
    }
}


[Serializable]
enum Locater    
{
    UPWARD, SIDEBYSIDE, FROMMIDDLE
}