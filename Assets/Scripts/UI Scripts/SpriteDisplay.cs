using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteDisplay : MonoBehaviour
{
    private Image _spriteToDisplay;

    void Start()
    {
        _spriteToDisplay = GetComponent<Image>();
    }

    public void UpdateDisplay(Sprite sprite)
    {
        _spriteToDisplay.sprite = sprite;
    }
}
