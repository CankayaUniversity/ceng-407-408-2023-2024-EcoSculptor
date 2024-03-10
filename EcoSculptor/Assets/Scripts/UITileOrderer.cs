using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class UITileOrderer : MonoBehaviour
{
    [SerializeField] private float spaceXDirection = 150f;
    [SerializeField] private float spaceYDirection = 80f;

    [SerializeField] private Locater locater;
    [SerializeField] private float startingXPos;
    

    private Tween _newTween;
    private void OnEnable()
    {
        var iteration = 1;
        foreach (RectTransform child in transform)
        {
            child.localPosition = Vector3.zero;
            var newPos = child.position;
            switch (locater)
            {
                case Locater.UPWARD:
                    newPos.y += spaceYDirection * iteration++;
                    break;
                
                case Locater.SIDEBYSIDE:
                    newPos.x += spaceXDirection * iteration++;
                    break;
                
                case Locater.FROMMIDDLE:
                    newPos.x += startingXPos;
                    newPos.x += spaceXDirection * iteration++;
                    newPos.y += spaceYDirection;
                    break;
            }
            Debug.Log(child.name + "___" + newPos);

            //_newTween?.Kill();
            _newTween = child.DOMove(newPos, .2f);
        }
    }
}

[Serializable]
enum Locater    
{
    UPWARD, SIDEBYSIDE, FROMMIDDLE
}