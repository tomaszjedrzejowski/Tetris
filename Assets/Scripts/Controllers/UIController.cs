using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Action onStartClick;
    public Action onRestartClick;

    [SerializeField] private TetraminoSpawner spawner;
    [SerializeField] private TextDisplay levelDisplay;
    [SerializeField] private TextDisplay linesDisplay;
    [SerializeField] private SpriteDisplay spriteDisplay;
    [SerializeField] private PopUp gameOverPopUp;
    [SerializeField] private Button startButton;
    [SerializeField] private string startButtonText = "Start";
    [SerializeField] private string restartButtonText = "Restart";

    private Text _startButtonTextComponnent;
    private bool _isGameloopActive;

    private void Start()
    { 
        spawner.OnNextTetraminoSelect += HandleTetraminoListUpdate;
        _startButtonTextComponnent = startButton.GetComponentInChildren<Text>();
        SetPopUpActive(gameOverPopUp, false);
    }

    private void OnDisable()
    {
        spawner.OnNextTetraminoSelect -= HandleTetraminoListUpdate;
    }

    public void SetGameloopActiveFlag(bool isActive)
    {
        _isGameloopActive = isActive;
    }
    public void HandleGameOver()
    {
        SetPopUpActive(gameOverPopUp, true);
    }

    public void HandleGameReset()
    {
        SetPopUpActive(gameOverPopUp, false);
    }

    public void HandleLineCompleted(int value)
    {
        linesDisplay.UpdateDisplay(value);
    }

    public void HandleLevelChange(int value)
    {
        levelDisplay.UpdateDisplay(value);
    }

    private void HandleTetraminoListUpdate(Tetramino tetramino)
    {
        Sprite sprite = tetramino.GetSprite();
        spriteDisplay.UpdateDisplay(sprite);
    }

    public void OnStartButtonClick()
    {
        if (_isGameloopActive == false)
        {
            onStartClick?.Invoke();            
            _startButtonTextComponnent.text = restartButtonText;
        }
        else if (_isGameloopActive == true)
        {
            onRestartClick?.Invoke();
            _startButtonTextComponnent.text  = startButtonText;        
        }
    }

    private void SetPopUpActive(PopUp popUp, bool isActive)
    {
        popUp.gameObject.SetActive(isActive);
    }
}
