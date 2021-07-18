using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteDisplay : MonoBehaviour
{
    [SerializeField] Sprite Itetramino, Jtetramino, Ltetramino, Otetramino, Stetramino, Ttetramino, Ztetramino;
    private Image _spriteToDisplay;

    private Dictionary<tetraminos, Sprite> tetraminoSprites;

    void Start()
    {
        _spriteToDisplay = GetComponent<Image>();
        tetraminoSprites = new Dictionary<tetraminos, Sprite> { {tetraminos.I, Itetramino}, {tetraminos.J, Jtetramino }, { tetraminos.L, Ltetramino},
            { tetraminos.O, Otetramino}, { tetraminos.S, Stetramino}, { tetraminos.T, Ttetramino}, { tetraminos.Z, Ztetramino} };
    }

    public void UpdateDisplay(object tetraminoKey)
    {
        if (tetraminoSprites.ContainsKey((tetraminos)tetraminoKey))
        {
            var sprite = tetraminoSprites[(tetraminos)tetraminoKey];
            _spriteToDisplay.sprite = sprite;
        }
    }
}
