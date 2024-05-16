using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTileIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeSprite(Image image) { _image.sprite = image.sprite; }
}
